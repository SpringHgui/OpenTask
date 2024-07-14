// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Internal;
using MQTTnet.Protocol;
using MQTTnet.Server;
using Newtonsoft.Json;
using OpenTask.Application.Core.Interface;
using OpenTask.Application.Core.Models;
using OpenTask.Core;
using System.Collections.Concurrent;
using System.Net;
using System.Text;

namespace OpenTask.Application.Core
{
    public class OpenTaskServer : Disposable, ITaskServer
    {
        /// <summary>
        /// 唯一标识，标识每个server实例，且每次启动都会可能
        /// </summary>
        public string Identifier { get; private set; }

        public string ExternalUrl => options.ExternalUrl ?? $"{options.Ip}:{options.Port}";

        public ConcurrentDictionary<string, ExecutorClient> CurrentNodeOnlineUsers { private set; get; } = new ConcurrentDictionary<string, ExecutorClient>();

        public ConcurrentDictionary<string, ExecutorClient> CurrentNodeOnlineServer { private set; get; } = new ConcurrentDictionary<string, ExecutorClient>();

        public ConcurrentDictionary<string, IEnumerable<ExecutorClient>> OtherNodeOlineUsers { get; private set; } = new ConcurrentDictionary<string, IEnumerable<ExecutorClient>>();

        private MqttServer mqttServer;
        public IDiscovery Discovery { get; private set; }

        private readonly TaskServerOptions options;
        private readonly ILogger<OpenTaskServer> logger;
        private readonly SelfSubscriber selfSubscriber;
        private readonly ITaskSchedulingHandler schedulingHandler;

        [Obsolete]
        public OpenTaskServer(
            IOptions<TaskServerOptions> options, IDiscovery discovery, ILogger<OpenTaskServer> logger,
            ITaskSchedulingHandler schedulingHandler, IServiceProvider serviceProvider)
        {
            string idFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Identifier");
            if (File.Exists(idFile))
            {
                Identifier = File.ReadAllText(idFile);
            }
            else
            {
                Identifier = Environment.MachineName + "@" + Guid.NewGuid().ToString();
                File.WriteAllText(idFile, Identifier);
            }

            Console.WriteLine(ConstString.LOGO);

            logger.LogInformation($"[OpenTask] 启动，节点id={Identifier}");
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            Discovery = discovery ?? throw new ArgumentNullException(nameof(discovery));

            this.logger = logger;
            this.schedulingHandler = schedulingHandler;

            if (this.options.Ip == null)
            {
                string strHostName = string.Empty;
                IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress[] addr = ipEntry.AddressList.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToArray();
                strHostName = addr.First().ToString();

                this.options.Ip = strHostName;
            }

            Discovery.OnSloting += Discovery_OnSloting;
            Discovery.OnSloted += Discovery_OnSloted;

            ILogger<SelfSubscriber>? selfLogger = serviceProvider.GetService<ILogger<SelfSubscriber>>();

            selfSubscriber = new SelfSubscriber(this, selfLogger, serviceProvider);

            configMqtt();
        }

        private void configMqtt()
        {
            if (mqttServer != null)
            {
                throw new Exception("MqttServer has configed already!");
            }

            MqttServerOptions mqttServerOptions = new MqttServerOptionsBuilder()
                .WithDefaultEndpoint()
                .WithKeepAlive()
                .WithDefaultEndpointPort(options.Port)
                .Build();

            mqttServerOptions.EnablePersistentSessions = true;

            //mqttServer = new MqttFactory().CreateMqttServer(mqttServerOptions, new MyLog());
            mqttServer = new MqttFactory().CreateMqttServer(mqttServerOptions);
            string Filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RetainedMessages.json");
            if (File.Exists(Filename))
            {
                File.Delete(Filename);
            }

            mqttServer.RetainedMessageChangedAsync += e =>
            {
                logger.LogInformation($"[持久消息] 变化 {e.StoredRetainedMessages.Count}");

                string? directory = Path.GetDirectoryName(Filename);
                if (!Directory.Exists(directory))
                {
                    _ = Directory.CreateDirectory(directory);
                }

                if (e.StoredRetainedMessages != null)
                {
                    File.WriteAllText(Filename, JsonConvert.SerializeObject(e.StoredRetainedMessages.Where(x => x.PayloadSegment != null)));
                }
                else
                {
                    File.Delete(Filename);
                }

                return CompletedTask.Instance;
            };

            mqttServer.RetainedMessagesClearedAsync += e =>
            {
                logger.LogInformation($"[持久消息] 清除");
                File.Delete(Filename);
                return CompletedTask.Instance;
            };

            mqttServer.LoadingRetainedMessageAsync += e =>
            {
                logger.LogInformation($"[持久消息] 加载");
                List<MqttApplicationMessage> retainedMessages = [];
                if (File.Exists(Filename))
                {
                    string json = File.ReadAllText(Filename);
                    retainedMessages = JsonConvert.DeserializeObject<List<MqttApplicationMessage>>(json);
                }
                else
                {
                    retainedMessages = [];
                }

                e.LoadedRetainedMessages = retainedMessages;

                return CompletedTask.Instance;
            };

            // 发布拦截
            mqttServer.InterceptingPublishAsync += e =>
            {
                //logger.LogInformation($"[InterceptingPublish] 1");
                //if (MqttTopicFilterComparer.Compare(e.ApplicationMessage.Topic, "/myTopic/WithTimestamp/#") == MqttTopicFilterCompareResult.IsMatch)
                //{
                //    // Replace the payload with the timestamp. But also extending a JSON 
                //    // based payload with the timestamp is a suitable use case.
                //    e.ApplicationMessage.PayloadSegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes(DateTime.Now.ToString("O")));
                //}

                //if (e.ApplicationMessage.Topic == "not_allowed_topic")
                //{
                //    e.ProcessPublish = false;
                //    e.CloseConnection = true;
                //}

                return CompletedTask.Instance;
            };

            mqttServer.ClientConnectedAsync += MqttServer_ClientConnectedAsync;
            mqttServer.ClientDisconnectedAsync += MqttServer_ClientDisconnectedAsync;

            // 连接检查
            mqttServer.ValidatingConnectionAsync += e =>
            {
                if (e.ClientId == "SpecialClient")
                {
                    if (e.UserName != "USER" || e.Password != "PASS")
                    {
                        e.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                    }
                }

                e.ResponseUserProperties = [new MQTTnet.Packets.MqttUserProperty("server", Identifier)];

                return CompletedTask.Instance;
            };

            // 订阅拦截
            mqttServer.InterceptingSubscriptionAsync += e =>
            {
                logger.LogInformation($"[订阅事件过滤器] {e.ClientId} 订阅了 {e.TopicFilter.Topic} 主题");
                //if (e.TopicFilter.Topic.StartsWith("admin/foo/bar") && e.ClientId != "theAdmin")
                //{
                //    e.Response.ReasonCode = MqttSubscribeReasonCode.ImplementationSpecificError;
                //}

                //if (e.TopicFilter.Topic.StartsWith("the/secret/stuff") && e.ClientId != "Imperator")
                //{
                //    e.Response.ReasonCode = MqttSubscribeReasonCode.ImplementationSpecificError;
                //    e.CloseConnection = true;
                //}

                return CompletedTask.Instance;
            };

            mqttServer.InterceptingPublishAsync += e =>
            {
                logger.LogInformation($"[事件发布过滤器] {e.ClientId} 发布了 {e.ApplicationMessage.Topic}");
                string payloadText = string.Empty;
                if (e.ApplicationMessage.PayloadSegment.Count > 0)
                {
                    payloadText = Encoding.UTF8.GetString(
                        e.ApplicationMessage.PayloadSegment.Array,
                        e.ApplicationMessage.PayloadSegment.Offset,
                        e.ApplicationMessage.PayloadSegment.Count);
                }

                MqttNetConsoleLogger.PrintToConsole($"'{e.ClientId}' reported '{e.ApplicationMessage.Topic}' > '{payloadText}'", ConsoleColor.Magenta);
                return CompletedTask.Instance;
            };

            //options.ApplicationMessageInterceptor = c =>
            //{
            //    if (c.ApplicationMessage.Payload == null || c.ApplicationMessage.Payload.Length == 0)
            //    {
            //        return;
            //    }

            //    try
            //    {
            //        var content = JObject.Parse(Encoding.UTF8.GetString(c.ApplicationMessage.Payload));
            //        var timestampProperty = content.Property("timestamp");
            //        if (timestampProperty != null && timestampProperty.Value.Type == JTokenType.Null)
            //        {
            //            timestampProperty.Value = DateTime.Now.ToString("O");
            //            c.ApplicationMessage.Payload = Encoding.UTF8.GetBytes(content.ToString());
            //        }
            //    }
            //    catch (Exception)
            //    {
            //    }
            //};

            mqttServer.ClientConnectedAsync += e =>
            {
                return CompletedTask.Instance;
            };
        }

        public IEnumerable<ExecutorClient> GetAllClientsOnline()
        {
            IEnumerable<ExecutorClient> clients = CurrentNodeOnlineUsers.Select(x => x.Value);
            foreach (KeyValuePair<string, IEnumerable<ExecutorClient>> item in OtherNodeOlineUsers)
            {
                clients = clients.Concat(item.Value);
            }

            return clients;
        }

        public IEnumerable<ExecutorClient> GetClientsByAppName(string appname)
        {
            IEnumerable<ExecutorClient> clients = CurrentNodeOnlineUsers.Where(x => x.Value.GroupName == appname).Select(x => x.Value);
            foreach (KeyValuePair<string, IEnumerable<ExecutorClient>> item in OtherNodeOlineUsers)
            {
                clients = clients.Concat(item.Value.Where(x => x.GroupName == appname));
            }

            return clients;
        }

        private async Task MqttServer_ClientDisconnectedAsync(ClientDisconnectedEventArgs arg)
        {
            logger.LogInformation($"客户端离线 {arg.ClientId}");
            if (CurrentNodeOnlineUsers.TryRemove(arg.ClientId, out ExecutorClient? client))
            {
                logger.LogInformation("客户端移除成功");
            }
            else
            {
                logger.LogError($"客户端未找到 {arg.ClientId}");
            }

            MqttApplicationMessage applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic($"server/from/{Identifier}/clients-change")
                .WithPayload(System.Text.Json.JsonSerializer.Serialize(CurrentNodeOnlineUsers.Select(x => x.Value)))
                .WithRetainFlag(true)
                .Build();

            _ = await selfSubscriber.PublishAsync(applicationMessage);
        }

        private async Task MqttServer_ClientConnectedAsync(ClientConnectedEventArgs arg)
        {
            string? from = arg.UserProperties?.Where(x => x.Name == "from").FirstOrDefault()?.Value;

            if (from == null)
            {
                // todo: close
                return;
            }

            var clinet = new ExecutorClient()
            {
                ServerId = Identifier,
                GroupName = null,
                ClientId = arg.ClientId,
                StartTime = DateTime.Now,
            };

            if (from == "client")
            {
                string? appName = arg.UserProperties?.Where(x => x.Name == ConstString.UserProperties.APP_NAME).FirstOrDefault()?.Value;

                if (appName == null)
                {
                    // todo: close
                    return;
                }

                clinet.GroupName = appName;

                // 有新的工作节点连接到集群，发布消息
                CurrentNodeOnlineUsers.TryAdd(arg.ClientId, clinet);

                // 这个需要在任何节点订阅后，立即收到最后一次数据
                MqttApplicationMessage applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic($"server/from/{Identifier}/clients-change")
                    .WithPayload(System.Text.Json.JsonSerializer.Serialize(CurrentNodeOnlineUsers.Select(x => x.Value)))
                    .WithRetainFlag(true)
                    .Build();

                await PublishAsync(applicationMessage);
            }
            else
            {
                logger.LogInformation($"服务端订阅当前节点 {arg.ClientId}");
                CurrentNodeOnlineServer.TryAdd(arg.ClientId, clinet);
            }
        }

        public async Task<MqttClientPublishResult> PublishAsync(MqttApplicationMessage applicationMessage, CancellationToken cancellationToken = default)
        {
            return await selfSubscriber.PublishAsync(applicationMessage, cancellationToken);
        }

        public async Task StopAsync()
        {
            await mqttServer.StopAsync();
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            // 以下执行顺序不可颠倒
            await mqttServer.StartAsync();
            _ = await selfSubscriber.StartAsync(stoppingToken);

            await schedulingHandler.Start(this, stoppingToken);

            await Discovery.StartAsync(this, stoppingToken);
        }

        private async Task Discovery_OnSloting(IDiscovery sender, IEnumerable<Domain.Servers.OpenTaskServer> mqttNodes)
        {
            logger.LogInformation($"[OnSloting] ");
            await StopAllDispatch();
        }

        private async Task Discovery_OnSloted(IDiscovery sender, IEnumerable<Domain.Servers.OpenTaskServer> mqttNodes)
        {
            logger.LogInformation($"[OnSloted] ");
            await RestartAllDispatch();
        }

        private async Task StopAllDispatch()
        {
            StopDispatch();

            // 这个需要在任何节点订阅后，立即收到最后一次数据
            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic($"cluster/from/{Identifier}/stop")
                .WithPayload(System.Text.Json.JsonSerializer.Serialize(CurrentNodeOnlineUsers.Select(x => x.Value)))
                .Build();

            await publishToCluster(applicationMessage);
        }

        private async Task RestartAllDispatch()
        {
            StartDispatch();

            // 这个需要在任何节点订阅后，立即收到最后一次数据
            MqttApplicationMessage applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic($"cluster/from/{Identifier}/start")
                .WithPayload(System.Text.Json.JsonSerializer.Serialize(CurrentNodeOnlineUsers.Select(x => x.Value)))
                .Build();

            await publishToCluster(applicationMessage);
        }

        private Task publishToCluster(MqttApplicationMessage applicationMessage)
        {
            IList<Task> tasks = [];
            foreach (var item in Discovery.clusterSubscribers)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    var res = item.Value.PublishAsync(applicationMessage).ConfigureAwait(false).GetAwaiter().GetResult();
                }));
            }

            return Task.WhenAll(tasks.ToArray());
        }

        public void StartDispatch()
        {
            logger.LogInformation("[开始调度]");
            // 启动定时拉取任务的timer

            schedulingHandler.StartDispatch();
        }

        public void StopDispatch()
        {
            logger.LogInformation("[停止调度]");
            // 清空时间轮，并暂停定时拉取任务的timer

            schedulingHandler.StopDispatch();
        }
    }
}
