// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using OpenTask.Application.Core.Interface;
using OpenTask.Core.Models;
using OpenTask.Domain.TaskInfos;
using OpenTask.Domain.TaskLogs;
using System.Text;
using System.Text.Json;

namespace OpenTask.Application.Core
{
    /// <summary>
    /// 1. 像客户端一样订阅自己，以实现客户端与自己进行通信
    /// 2. 其他server节点与当前节点通信
    /// </summary>
    public class SelfSubscriber
    {
        private readonly ITaskServer mqttServer;
        private readonly MqttClientOptions clientOptions;
        private readonly IMqttClient client;
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<SelfSubscriber> logger;

        [Obsolete]
        public SelfSubscriber(ITaskServer myMqttServer, ILogger<SelfSubscriber> logger, IServiceProvider serviceProvider)
        {
            mqttServer = myMqttServer;
            this.serviceProvider = serviceProvider;
            this.logger = logger;

            MqttFactory factory = new();
            MqttClientTcpOptions tcpOptions = new()
            {
                Port = int.Parse(mqttServer.ExternalUrl.Split(':')[1]),
                Server = mqttServer.ExternalUrl.Split(':')[0]
            };

            clientOptions = new MqttClientOptions
            {
                KeepAlivePeriod = TimeSpan.FromSeconds(10),
                ProtocolVersion = MQTTnet.Formatter.MqttProtocolVersion.V500,
                ClientId = mqttServer.Identifier,
                // new MqttClientWebSocketOptions { Uri = server };
                ChannelOptions = tcpOptions,
                Credentials = new MqttClientCredentials("", Encoding.UTF8.GetBytes("")),
                WillDelayInterval = 5,
                WillPayload = Encoding.UTF8.GetBytes($"Offline"),
            };

            client = factory.CreateMqttClient();

            client.ApplicationMessageReceivedAsync += e =>
            {
                string payloadText = string.Empty;
                if (e.ApplicationMessage.PayloadSegment.Count > 0)
                {
                    payloadText = Encoding.UTF8.GetString(
                        e.ApplicationMessage.PayloadSegment.Array,
                        e.ApplicationMessage.PayloadSegment.Offset,
                        e.ApplicationMessage.PayloadSegment.Count);
                }

                logger.LogInformation($"### RECEIVED APPLICATION MESSAGE {nameof(SelfSubscriber)} ###");
                logger.LogInformation($"+ Topic = {e.ApplicationMessage.Topic}");
                logger.LogInformation($"+ Payload = {payloadText}");
                logger.LogInformation($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                logger.LogInformation($"+ Retain = {e.ApplicationMessage.Retain}");

                string topic = e.ApplicationMessage.Topic.Split("/").Last();

                switch (topic)
                {
                    case "SyncHandlers":
                        string id = e.ApplicationMessage.UserProperties?.First(x => x.Name == "id").Value ?? throw new ArgumentNullException();
                        string[] data = JsonSerializer.Deserialize<string[]>(payloadText) ?? throw new ArgumentNullException();
                        if (mqttServer.CurrentNodeOnlineUsers.TryGetValue(id, out ExecutorClient? client))
                        {
                            client.Handelrs = data;

                            // 这个需要在任何节点订阅后，立即受到最后一次数据
                            MqttApplicationMessage msg = new MqttApplicationMessageBuilder()
                                .WithTopic($"server/from/{myMqttServer.Identifier}/clients-change")
                                .WithPayload(JsonSerializer.Serialize(myMqttServer.CurrentNodeOnlineUsers.Select(x => x.Value)))
                                .WithRetainFlag(true)
                                .Build();

                            _ = PublishAsync(msg);
                        }
                        else
                        {
                            logger.LogError($"未找到对应客户端 {id}");
                        }

                        break;
                    case "job_reslut":
                        OnTaskRequest? onJob = JsonSerializer.Deserialize<OnTaskRequest>(payloadText);
                        if (onJob == null)
                        {
                            throw new ArgumentNullException("解析失败");
                        }

                        using (IServiceScope scope = serviceProvider.CreateScope())
                        {
                            ITaskLogRepository taskService = scope.ServiceProvider.GetRequiredService<ITaskLogRepository>();
                            ITaskInfoRepository jobService = scope.ServiceProvider.GetRequiredService<ITaskInfoRepository>();

                            TaskLog task = taskService.GetTaskById(onJob.Job.TaskId);
                            if (task == null)
                            {
                                throw new Exception("task实例不存在");
                            }

                            task.HandleResult = onJob.ErrMsg;
                            task.HandleEnd = DateTime.Now;

                            task.HandleStatus = onJob.Success ? (sbyte)(int)TaskLogStatus.SUCCESS : (sbyte)(int)TaskLogStatus.FAIL;

                            taskService.Update(task);
                            TaskInfo job = jobService.GetJob(onJob.Job.JobId);
                            if (job == null)
                            {
                                throw new Exception($"任务不存在：{onJob.Job.JobId}");
                            }

                            jobService.UpdateParallelCount(onJob.Job.JobId, -1);

                            //// 报警
                            //if (!onJob.Success)
                            //{

                            //    if (job != null && job.AlarmType > 0)
                            //    {
                            //        switch (job.AlarmType)
                            //        {
                            //            case 1:
                            //                if (!string.IsNullOrEmpty(job.AlarmContent))
                            //                {
                            //                    var text = $"""
                            //        任务调度平台/调度失败提醒

                            //        任务名称：{job.GroupName}/{job.Name}
                            //        调度实例：TaskId：{onJob.Job.TaskId}
                            //        危险级别：{"高"}
                            //        提醒时间：{DateTime.Now.ToString("MM-dd HH:mm:ss")}
                            //        详细内容：{onJob.ErrMsg}

                            //        请值班研发人员查看失败原因，及时处理！
                            //        """;

                            //                    var robot = new RobotApi(job.AlarmContent);
                            //                    robot.Send(new SendMsgRequest
                            //                    {
                            //                        text = new SendMsgRequest.Text
                            //                        {
                            //                            content = text
                            //                        },
                            //                        msgtype = SendMsgRequest.MsgType.text
                            //                    });
                            //                }

                            //                break;
                            //            default:

                            //                break;
                            //        }
                            //    }
                            //}
                        }
                        break;
                    case "proxy":
                        ProxyModel? proxy = JsonSerializer.Deserialize<ProxyModel>(payloadText);
                        if (proxy == null)
                        {
                            throw new Exception("解析失败");
                        }

                        MqttApplicationMessage applicationMessage = new MqttApplicationMessageBuilder()
                          .WithTopic(proxy.topic)
                          .WithPayload(proxy.data)
                        .Build();

                        MqttClientPublishResult res = mqttServer.PublishAsync(applicationMessage).Result;
                        break;
                    case "stop_disp":
                        mqttServer.StopDispatch();
                        break;
                    case "start_disp":
                        mqttServer.StartDispatch();
                        break;
                    default:
                        throw new Exception("self 未知的主题");
                }

                return Task.CompletedTask;
            };

            client.ConnectedAsync += async e =>
            {
                // server/316bc382-e64d-4ce6-a82e-c3c535974074/proxy
                string topic = $"client/from/+/#";
                _ = await client.SubscribeAsync(topic);

                string topicCuster = $"cluster/from/+/#";
                _ = await client.SubscribeAsync(topicCuster);

                logger.LogInformation($"[订阅自己] 成功 {topic}");
            };

            client.DisconnectedAsync += async e =>
            {
                try
                {
                    logger.LogInformation($"[断开自己] {e.ClientWasConnected}|{e.Reason}|{e.ReasonString}|{e.Exception?.Message}");
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    _ = await client.ConnectAsync(clientOptions);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "离线事件处理异常");
                }
            };
        }

        /// <summary>
        /// 服务端发布指令唯一入口
        /// 1
        /// </summary>
        /// <param name="applicationMessage"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<MqttClientPublishResult> PublishAsync(MqttApplicationMessage applicationMessage, CancellationToken cancellationToken = default)
        {
            return await client.PublishAsync(applicationMessage, cancellationToken);
        }

        public async Task<MqttClientConnectResult> StartAsync(CancellationToken stoppingToken)
        {
            MqttClientConnectResult res = await client.ConnectAsync(clientOptions);
            if (res.ResultCode == MqttClientConnectResultCode.Success)
            {
                logger.LogInformation("自己连接自己成功");
            }
            else
            {
                throw new Exception($"mqtt自己连接自己失败了，请检查 clientOptions.ChannelOptions");
            }

            return res;
        }
    }
}
