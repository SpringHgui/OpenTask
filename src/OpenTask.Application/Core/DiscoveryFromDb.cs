// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using OpenTask.Application.Core.Interface;
using OpenTask.Application.Lockers;
using OpenTask.Domain.Lockers;
using OpenTask.Domain.Servers;
using System.Collections.Concurrent;
using System.Timers;
using System.Xml.Linq;
using static OpenTask.Application.Core.Interface.IDiscovery;

namespace OpenTask.Application.Core
{
    public class DiscoveryFromDb : IDiscovery
    {
        private readonly IServiceProvider service;
        private readonly System.Timers.Timer discoveryTimer;
        private int times = 0;
        private readonly ILogger<DiscoveryFromDb> logger;
        private ITaskServer myMqttServer;

        //public event OnNewNodeChange OnNewNodeConnected;
        //public event OnNewNodeChange OnNodeDisconnected;
        public event OnNodeSlotsChange OnSlotsChange;
        public event OnSlotParamer OnSloting;
        public event OnSlotParamer OnSloted;

        public ConcurrentDictionary<string, ClusterSubscriber> clusterSubscribers { get; private set; } = new();

        /// <summary>
        /// 服务注册间隔 单位：s
        /// </summary>
        private const int HEART_INTERVAL = 5;

        [Obsolete]
        public DiscoveryFromDb(IServiceProvider service, ILogger<DiscoveryFromDb> logger)
        {
            this.logger = logger;

            this.service = service;
            discoveryTimer = new System.Timers.Timer
            {
                Interval = HEART_INTERVAL * 1000
            };

            discoveryTimer.Elapsed += discoveryTimerElapsed;
        }

        public Task StartAsync(ITaskServer myMqttServer, CancellationToken stoppingToken)
        {
            this.myMqttServer = myMqttServer;

            // 立刻执行一次
            Register();

            // 定时检测
            discoveryTimer.Start();
            return Task.CompletedTask;
        }

        [Obsolete]
        private void discoveryTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            discoveryTimer.Enabled = false;
            times++;

            try
            {
                // 心跳写入数据库
                Register();

                // 两倍心跳间隔的时间进行服务发现
                if (times % 2 == 0)
                {
                    TryReslot().Wait();
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex, "服务发现处理异常");
                logger.LogError(ex, null);
            }
            finally
            {
                discoveryTimer.Enabled = true;
            }
        }

        [Obsolete]
        private async Task updateSubscriber(IEnumerable<Domain.Servers.OpenTaskServer> latestNodes)
        {
            logger.LogDebug($"[updateSubscriber] 节点数：{latestNodes.Count()}");
            foreach (Domain.Servers.OpenTaskServer item in latestNodes)
            {
                // 新增的节点
                if (!clusterSubscribers.ContainsKey(item.ServerId))
                {
                    await OnNewNodeConnected(item);
                }
            }

            foreach (KeyValuePair<string, ClusterSubscriber> item in clusterSubscribers)
            {
                // 离线的节点
                if (!latestNodes.Any(x => x.ServerId == item.Key))
                {
                    await OnNodeDisconnected(item.Key);
                }
            }
        }

        [Obsolete]
        private async Task OnNewNodeConnected(Domain.Servers.OpenTaskServer node)
        {
            if (node.ServerId == myMqttServer.Identifier)
            {
                await Task.CompletedTask;
                return;
            }

            ILogger<ClusterSubscriber> subscriberLogger = service.GetRequiredService<ILogger<ClusterSubscriber>>();
            ClusterSubscriber sub = new(node, subscriberLogger, myMqttServer);
            sub.OnClientsChange += OnClientsChange;
            _ = clusterSubscribers.TryAdd(sub.Guid, sub);
            await sub.StartAsync();
        }

        private async Task OnNodeDisconnected(string nodeGuid)
        {
            if (clusterSubscribers.Remove(nodeGuid, out ClusterSubscriber? subscriber))
            {
                await subscriber.StopAsync();
            }
        }

        // 
        public void OnClientsChange(IEnumerable<ExecutorClient> clients, Domain.Servers.OpenTaskServer nodeInfo)
        {
            _ = myMqttServer.OtherNodeOlineUsers.AddOrUpdate(nodeInfo.ServerId, _ => clients, (key, _) => clients);
        }

        private const int TOTAL_SLOT_COUNT = 16384;
        private const string LOCK_KEY = "sys_reslot";


        /// <summary>
        /// 检查在线的servers是否恰好是完整的slot
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IsWholeSlot(IEnumerable<Domain.Servers.OpenTaskServer> latestNodes)
        {
            // 检查在线节点是否是完整的Slot
            IOrderedEnumerable<Domain.Servers.OpenTaskServer> orderedNodes = latestNodes.OrderBy(x => x.SlotFrom);
            int index = 0;
            foreach (Domain.Servers.OpenTaskServer? item in orderedNodes)
            {
                logger.LogDebug($"[检查] {item.ServerId}：{item.SlotFrom}~{item.SlotEnd}");

                if (item.SlotFrom != index)
                {
                    return false;
                }

                index = item.SlotEnd + 1;
            }

            logger.LogDebug($"[检查完毕] index={index}");
            return TOTAL_SLOT_COUNT == index;
        }

        /// <summary>
        /// 重建Slot
        /// 保证同一时间只能有一个节点进行该操作
        /// </summary>
        /// <param name="mqttNodes"></param>
        [Obsolete]
        private async Task TryReslot()
        {
            using IServiceScope scope = service.CreateScope();
            LockerService lockerService = scope.ServiceProvider.GetRequiredService<LockerService>();

            logger.LogInformation($"当前监听节点列表：{String.Join(",", clusterSubscribers.Select(x => x.Key))}");

            // 服务发现
            IEnumerable<Domain.Servers.OpenTaskServer> latestNodes = FindAllOnlineServer();
            await updateSubscriber(latestNodes);

            // 1分钟内最多只有一个节点执行一次
            if (!lockerService.TryLock(LOCK_KEY, myMqttServer.Identifier, 60, out Locker? locker))
            {
                logger.LogDebug($"[获取锁失败]:{locker?.Version}@{locker?.LockedAt}");
                return;
            }

            try
            {
                if (IsWholeSlot(latestNodes))
                {
                    return;
                }

                logger.LogInformation($"[集群节点分配不均] 开始重新分配slot，当前节点数：{latestNodes.Count()}");

                // TODO: 存在通知失败的可能，要比对连接到当前server的其他的server，
                // 与服务发现的是否一致，如果不一致则可能无法通知到对应的server，存在任务重复调度的可能，打印警告日志
                await OnSloting.Invoke(this, latestNodes);

                latestNodes = CalculateSlot(latestNodes);

                IServerRepository serverService = scope.ServiceProvider.GetRequiredService<IServerRepository>();
                serverService.UpdateSlot(latestNodes.Select(x => new Domain.Servers.OpenTaskServer
                {
                    SlotFrom = x.SlotFrom,
                    SlotEnd = x.SlotEnd,
                    EndPoint = x.EndPoint,
                    ServerId = x.ServerId,
                    HeartAt = x.HeartAt,
                    Id = x.Id,
                }));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[ReSlot Error]");
            }
            finally
            {
                // TODO：通知存在失败情况，其他节点是否存在等待超时的机制，恢复reslot的暂停
                await OnSloted.Invoke(this, latestNodes);

                lockerService.Release(LOCK_KEY, myMqttServer.Identifier);
            }
        }

        public IEnumerable<Domain.Servers.OpenTaskServer> CalculateSlot(IEnumerable<Domain.Servers.OpenTaskServer> latestNodes)
        {
            Domain.Servers.OpenTaskServer[] nodes = latestNodes.OrderBy(x => x.Id).ToArray();
            int perCount = (TOTAL_SLOT_COUNT - nodes.Count()) / nodes.Count();
            int start = 0;

            for (int i = 0; i < nodes.Length; i++)
            {
                int next = start + perCount;
                nodes[i].SlotFrom = start;
                nodes[i].SlotEnd = i == nodes.Length - 1 ? TOTAL_SLOT_COUNT - 1 : next - 1;
                start = next;
            }

            return nodes;
        }

        public IEnumerable<Domain.Servers.OpenTaskServer> FindAllOnlineServer()
        {
            using IServiceScope scope = service.CreateScope();
            IServerRepository serverService = scope.ServiceProvider.GetRequiredService<IServerRepository>();

            IEnumerable<Domain.Servers.OpenTaskServer> list = serverService.GetServerOnline(HEART_INTERVAL * 3);

            return list;
        }

        public void Register()
        {
            using IServiceScope scope = service.CreateScope();
            IServerRepository serverService = scope.ServiceProvider.GetRequiredService<IServerRepository>();

            serverService.RegisterOrUpdate(new Domain.Servers.OpenTaskServer
            {
                ServerId = myMqttServer.Identifier,
                EndPoint = myMqttServer.ExternalUrl,
                HeartAt = DateTime.Now,
            });
        }

    }
}
