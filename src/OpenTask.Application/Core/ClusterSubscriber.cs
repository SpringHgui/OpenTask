// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Server;
using OpenTask.Application.Core.Interface;
using System.Text;
using System.Text.Json;

namespace OpenTask.Application.Core
{
    public class ClusterSubscriber
    {
        private readonly IMqttClient client;

        private readonly Domain.Servers.OpenTaskServer nodeInfo;
        private readonly MqttClientOptions clientOptions;
        private readonly ITaskServer mqttServer;

        public string Guid => nodeInfo.ServerId;

        private string willTopic => $"sys/cluster/offline/{Guid}";

        public event ClientsChange OnClientsChange;
        public delegate void ClientsChange(IEnumerable<ExecutorClient> clients, Domain.Servers.OpenTaskServer nodeInfo);

        private readonly ILogger<ClusterSubscriber> logger;

        public ClusterSubscriber(Domain.Servers.OpenTaskServer nodeInfo, ILogger<ClusterSubscriber> logger, ITaskServer mqttServer)
        {
            this.nodeInfo = nodeInfo;
            this.mqttServer = mqttServer;
            this.logger = logger;

            MqttFactory factory = new();

            clientOptions = new MqttClientOptions
            {
                KeepAlivePeriod = TimeSpan.FromSeconds(10),
                ProtocolVersion = MQTTnet.Formatter.MqttProtocolVersion.V500,
                ClientId = this.mqttServer.Identifier,
                ChannelOptions = new MqttClientTcpOptions // new MqttClientWebSocketOptions { Uri = server };
                {
                    Port = int.Parse(nodeInfo.EndPoint.Split(':')[1]),
                    Server = nodeInfo.EndPoint.Split(':')[0]
                },
                // TODO: 账号通过算法生产
                Credentials = new MqttClientCredentials("", Encoding.UTF8.GetBytes("")),
                WillTopic = willTopic,
                WillDelayInterval = 5,
                WillPayload = Encoding.UTF8.GetBytes($"Offline"),
                UserProperties =
                [
                    new MQTTnet.Packets.MqttUserProperty("from", "server")
                ]
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

                logger.LogDebug($"### ClusterSubscriber RECEIVED APPLICATION MESSAGE {nameof(ClusterSubscriber)}###");
                logger.LogDebug($"+ Topic = {e.ApplicationMessage.Topic}");
                logger.LogDebug($"+ Payload = {payloadText}");
                logger.LogDebug($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                logger.LogDebug($"+ Retain = {e.ApplicationMessage.Retain}");

                if (e.ApplicationMessage.Topic.EndsWith("clients-change"))
                {
                    if (string.IsNullOrEmpty(payloadText))
                    {
                        return Task.CompletedTask;
                    }

                    List<ExecutorClient>? clients = JsonSerializer.Deserialize<List<ExecutorClient>>(payloadText);
                    if (clients == null)
                    {
                        throw new Exception("解析客户端列表失败");
                    }

                    // 在这里赋值，减少重复的数据通过网络传输
                    foreach (ExecutorClient item in clients)
                    {
                        item.ServerId = nodeInfo.ServerId;
                    }

                    OnClientsChange?.Invoke(clients, nodeInfo);
                }
                else if (e.ApplicationMessage.Topic.EndsWith("proxy"))
                {
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
                    // TODO: ACK 确认收到了该消息
                }
                else if (e.ApplicationMessage.Topic.EndsWith("stop_disp"))
                {
                    mqttServer.StopDispatch();
                    // TODO: ACK 确认已经停止
                }
                else if (e.ApplicationMessage.Topic.EndsWith("start_disp"))
                {
                    mqttServer.StartDispatch();
                    // TODO: ACK 确认已经启动
                }
                else
                {
                    logger.LogInformation("ClusterSubscriber 未处理的处主题");
                }

                return Task.CompletedTask;
            };

            client.ConnectedAsync += async e =>
            {
                _ = await client.SubscribeAsync($"server/from/{Guid}/#");
                logger.LogInformation($"[{mqttServer.Identifier}] 订阅成功 {nodeInfo.ServerId}", null, null);
            };

            client.DisconnectedAsync += async e =>
            {
                logger.LogInformation("### DISCONNECTED FROM SERVER ###");
                await Task.Delay(TimeSpan.FromSeconds(5));

                try
                {
                    _ = await client.ConnectAsync(clientOptions);
                }
                catch
                {
                    logger.LogInformation("### RECONNECTING FAILED ###");
                }
            };
        }

        private async Task ConnectAsync()
        {
            try
            {
                try
                {
                    _ = await client.ConnectAsync(clientOptions);
                }
                catch (Exception exception)
                {
                    logger.LogInformation("### CONNECTING FAILED ###" + Environment.NewLine + exception);
                }

                logger.LogInformation("### WAITING FOR APPLICATION MESSAGES ###");
            }
            catch (Exception exception)
            {
                logger.LogError(exception, $"连接Server节点失败 {nodeInfo.ServerId}");
            }
        }

        public async Task StartAsync()
        {
            await ConnectAsync();
        }

        public async Task StopAsync()
        {
            _ = await client.TryDisconnectAsync();
            client.Dispose();
        }

        public async Task<MqttClientPublishResult> PublishAsync(MqttApplicationMessage applicationMessage)
        {
            return await client.PublishAsync(applicationMessage);
        }
    }
}