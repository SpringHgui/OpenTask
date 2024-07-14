// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Server;
using OpenTask.Core.Models;
using Polly;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace OpenTask.Core.Base
{
    public class TaskWorker : IOpenTaskWorker
    {
        private readonly ILogger<TaskWorker> logger;
        private readonly IMqttClient client;
        private readonly IServiceProvider serviceProvider;
        private readonly OpenWorkerOptions schedulerOptions;
        private readonly HandlerRegister schedulerConfig;
        private readonly ConcurrentQueue<OnTaskRequest> todoTasks = new();
        private readonly MqttClientOptions clientOptions;

        private string willTopic => $"sys/client/offline/{ClientId}";

        private readonly string ClientId;
        private readonly System.Timers.Timer timer;

        [Obsolete]
        public TaskWorker(
            ILogger<TaskWorker> logger,
            IOptions<OpenWorkerOptions> options,
            HandlerRegister schedulerConfig,
            IServiceProvider serviceProvider)
        {
            Console.WriteLine(ConstString.LOGO);

            this.schedulerConfig = schedulerConfig;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
            schedulerOptions = options.Value;
            schedulerOptions.Validate();

            timer = new System.Timers.Timer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 1000;
            timer.Start();

#if DEBUG
            MqttFactory factory = new(new MqttConsleLogger());
#else
            MqttFactory factory = new();
#endif
            ClientId = Guid.NewGuid().ToString();

            clientOptions = new MqttClientOptionsBuilder()
                .WithUserProperty(ConstString.UserProperties.APP_NAME, schedulerOptions.AppName)
                .WithUserProperty("from", "client")
                .WithClientId(ClientId)
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(10))
                .WithTcpServer(schedulerOptions.Addr.First().Split(':')[0], int.Parse(schedulerOptions.Addr.First().Split(':')[1]))
                .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V500)
                .WithWillTopic(willTopic).WithWillPayload(Encoding.UTF8.GetBytes($"Offline"))
                .WithWillDelayInterval(5)
                .Build();

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

                logger.LogInformation("### RECEIVED APPLICATION MESSAGE ###");
                logger.LogInformation($"+ Topic = {e.ApplicationMessage.Topic}");
                logger.LogInformation($"+ Payload = {payloadText}");
                logger.LogInformation($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                logger.LogInformation($"+ Retain = {e.ApplicationMessage.Retain}");

                // onjob
                string topic = e.ApplicationMessage.Topic.Split('/').Last();
                switch (topic)
                {
                    case "onjob":
                        OnTaskRequest? onjob = JsonSerializer.Deserialize<OnTaskRequest>(payloadText);

                        todoTasks.Enqueue(onjob);
                        logger.LogInformation($"当前待处理: {todoTasks.Count}");

                        break;
                    default:
                        break;
                }

                return Task.CompletedTask;
            };

            client.ConnectedAsync += async e =>
            {
                _ = await client.SubscribeAsync($"client/to/{ClientId}/#");

                MqttApplicationMessage applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic($"client/from/{ClientId}/SyncHandlers")
                    .WithUserProperty("id", ClientId)
                    .WithPayload(JsonSerializer.Serialize(schedulerConfig.handlers.Select(x => x.Key)))
                    .Build();

                await PublishAsync(applicationMessage, cancellationToken);
                logger.LogInformation($"[{ClientId}] 连接成功");
            };

            client.DisconnectedAsync += async e =>
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                await connectToServer(cancellationToken);
            };
        }

        private readonly CancellationToken cancellationToken;

        public async Task<MqttClientPublishResult> PublishAsync(
            MqttApplicationMessage mqttApplicationMessage, CancellationToken cancellationToken = default)
        {
            return await client.PublishAsync(mqttApplicationMessage, cancellationToken);
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("[OpenTask 启动]");

            await connectToServer(cancellationToken);
        }

        private async Task connectToServer(CancellationToken cancellationToken)
        {
            try
            {
                MqttClientConnectResult res = await client.ConnectAsync(clientOptions, cancellationToken);
                if (res.ResultCode == MqttClientConnectResultCode.Success)
                {
                    logger.LogInformation("[连接成功]");
                    return;
                }

                logger.LogError($"[连接失败] 😔😔😔 {res.ResultCode}/{res.ReasonString}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[连接异常]");
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Enabled = false;

            try
            {
                if (todoTasks.Count == 0)
                {
                    return;
                }

                logger.LogInformation($"本次需处理: {todoTasks.Count}");
                while (todoTasks.TryDequeue(out OnTaskRequest job))
                {
                    logger.LogInformation($"任务出队: {job.Job.TaskId}");

                    _ = DoTaskAsync(job);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "处理任务");
            }
            finally
            {
                timer.Enabled = true;
            }
        }

        // todo
        public void CancelTask()
        {

        }

        /// <summary>
        /// 正在执行的任务列表
        /// </summary>
        private readonly List<OnTaskRequest> ExecutingJobs = [];

        /// <summary>
        /// 执行job
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task DoTaskAsync(OnTaskRequest msg)
        {
            TodoTask job = msg.Job;

            try
            {
                ExecutingJobs.Add(msg);

                if (!schedulerConfig.handlers.TryGetValue(job.Content, out Type? jobType))
                {
                    throw new Exception($"jobName:{job.Name} -> {job.Content} 未注册");
                }

                ITaskHandler? jobHandler = serviceProvider.GetService(jobType) as ITaskHandler;

                logger.LogInformation($"[执行前] {job.Name}");

                TaskContext ctx = new(job);

                if (job.MaxAttempt > 0 && job.AttemptInterval > 0)
                {
                    Polly.Wrap.PolicyWrap policyWrap = Policy
                        .Wrap(Policy
                              .Handle<Exception>()
                              .Retry(job.MaxAttempt, async (exception, retryCount, context) =>
                              {
                                  logger.LogError($"[{job.Name}] 执行异常 {exception}");
                                  await Task.Delay(TimeSpan.FromSeconds(job.AttemptInterval));
                                  logger.LogInformation($"[{job.Name}] 开始第{retryCount}次重试");
                              }), Policy.Timeout(TimeSpan.FromHours(6)));

                    await policyWrap.Execute(async () =>
                     {
                         await jobHandler!.RunAsync(ctx);
                     });
                }
                else
                {
                    await jobHandler!.RunAsync(ctx);
                }

                logger.LogInformation($"[执行后] {job.Name}");

                msg.ErrMsg = "成功";
                msg.Success = true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[任务执行异常]");
                msg.Success = false;
                msg.ErrMsg = ex.Message;
            }
            finally
            {
                _ = ExecutingJobs.Remove(msg);
            }

            MqttApplicationMessage message = new MqttApplicationMessageBuilder()
                .WithTopic($"client/from/{ClientId}/job_reslut")
                .WithPayload(JsonSerializer.Serialize(msg))
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

            MqttClientPublishResult res = await PublishAsync(message, CancellationToken.None);
        }
    }
}
