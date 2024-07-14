// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MQTTnet;
using OpenTask.Application.Core.Interface;
using OpenTask.Core.Models;
using OpenTask.Domain.TaskInfos;
using OpenTask.Domain.TaskLogs;
using System.Collections.Concurrent;
using System.Text.Json;

namespace OpenTask.Application.Core
{
    public class TaskDispatcher : ITaskDispatcher
    {
        private readonly ILogger<TaskDispatcher> logger;
        private readonly IServiceProvider service;

        public TaskDispatcher(
            ILogger<TaskDispatcher> logger,
            IServiceProvider service)
        {
            this.logger = logger;
            this.service = service;
        }

        public ConcurrentDictionary<string, TaskCompletionSource<string>> Tcs = new();

        public async Task<(bool Success, string Message)> DispatchAsync(ITaskServer server, TaskInfo job)
        {
            using IServiceScope scope = service.CreateScope();
            TaskLog task = new()
            {
                TaskId = job.Id,
                HandleStart = DateTime.Now,
                HandleStatus = 1
            };


            ITaskInfoRepository jobService = scope.ServiceProvider.GetRequiredService<ITaskInfoRepository>();

            try
            {
                ITaskLogRepository taskService = scope.ServiceProvider.GetRequiredService<ITaskLogRepository>();

                List<ExecutorClient> groupClients = server.GetClientsByAppName(job.Appid).ToList();
                logger.LogInformation($"找到{groupClients.Count()}个 {job.Appid} 的执行节点 ");
                if (!groupClients.Any())
                {
                    string result = $"组[{job.Appid}]没有在线的执行器";
                    task.HandleResult = result;
                    task.HandleStatus = 3;

                    _ = taskService.AddTask(task);
                    return (false, result);
                }

                groupClients = groupClients.Where(x => x.Handelrs != null && x.Handelrs.Contains(job.Handler)).ToList();
                logger.LogInformation($"{groupClients.Count()}个执行节点支持 {job.Handler}");

                if (!groupClients.Any())
                {
                    string result = $"组[{job.Appid}]没有支持`{job.Handler}`的执行器";
                    task.HandleResult = result;
                    _ = taskService.AddTask(task);
                    return (false, result);
                }

                //if (job.MaxThread > 0)
                //{
                //    int count = 0;
                //    foreach (var groupClient in groupClients)
                //    {
                //        var id = Guid.NewGuid().ToString();
                //        TaskCompletionSource<string> taskCompletionSource = new TaskCompletionSource<string>();
                //        Tcs.TryAdd(id, taskCompletionSource);

                //        try
                //        {
                //            var clinet = server.GetClient(groupClient.ConnectionId);
                //            await clinet.SendAsync(nameof(OnInvoke), new OnInvoke
                //            {
                //                InvokeId = id,
                //                ApiName = "ExecutingJobs",
                //                Data = ""
                //            });

                //            var json = await taskCompletionSource.Task;
                //            var list = JsonSerializer.Deserialize<List<ExecutingJobsResponse>>(json);
                //            var runCount = list.Where(x => x.JobId == job.JobId).Count();

                //            count += runCount;
                //            LogHelper.Info($"{groupClient.ClientId}: {runCount}");
                //        }
                //        catch (Exception)
                //        {
                //            throw;
                //        }
                //        finally
                //        {
                //            Tcs.TryRemove(id, out _);
                //        }
                //    }

                //    if (count >= job.MaxThread)
                //    {
                //        return (false, $"并发数达限制 Max：{job.MaxThread} Current: {count}");
                //    }
                //}

                IEnumerable<ExecutorClient>? executors = null;
                if (job.ScheduleMode == "alone")
                {
                    // TODO：繁忙检查，选择一个当前相对空闲的客户端分配任务
                    int index = new Random().Next(0, 1000) % groupClients.Count();
                    executors = groupClients.Where(x => x.ConnectionId == groupClients[index].ConnectionId);
                }
                else
                {
                    executors = job.ScheduleMode == "sphere" ? (IEnumerable<ExecutorClient>)groupClients : throw new Exception($"运行模式参数异常: 未知的模式{job.ScheduleMode}");
                }

                logger.LogInformation($"以下节点需要下发通知 {JsonSerializer.Serialize(executors)}");

                foreach (ExecutorClient executor in executors)
                {
                    task.HandleClient = executor.ClientId;

                    //var clinet = server.GetClient(executor.ConnectionId);
                    //if (clinet == null)
                    //{
                    //    var result = $"ConnectionId: [{executor.ConnectionId}] 竟然不在线";
                    //    task.Result = result;
                    //    task.Flags |= (int)TaskLogStatus.FAIL;
                    //    taskService.AddTask(task);

                    //    // 这个分支不应该出现
                    //    return (false, result);
                    //}

                    task = taskService.AddTask(task);

                    logger.LogInformation($"任务创建完成: {task.TaskId}，clinetId：{executor.ClientId}");

                    jobService.UpdateParallelCount(job.Id, +1);

                    if (executor.ServerId == server.Identifier)
                    {
                        logger.LogInformation($"工作节点在当前服务器");
                        MqttApplicationMessage applicationMessage = new MqttApplicationMessageBuilder()
                           .WithTopic($"client/to/{executor.ClientId}/onjob")
                           .WithPayload(JsonSerializer.Serialize(new OnTaskRequest
                           {
                               Job = new OpenTask.Core.Models.TodoTask
                               {
                                   TaskId = task.TaskId,
                                   Name = job.Name,
                                   GroupName = job.Appid,
                                   TimeExpression = job.TimeConf,
                                   TimeType = job.TimeType,
                                   MaxAttempt = job.AttemptMax,
                                   AttemptInterval = job.AttemptInterval,
                                   Content = job.Handler,
                                   Description = job.Description,
                                   ExecuteMode = job.ScheduleMode,
                                   JobId = job.Id,
                                   JobParams = job.HandleParams
                               }
                           }))
                           .Build();

                        _ = await server.PublishAsync(applicationMessage);
                        taskService.UpdateTaskFlag(task.TaskId, (int)TaskLogStatus.Process);
                    }
                    else
                    {
                        logger.LogInformation($"工作节点在 {executor.ServerId} 开始发送转发主题");

                        // 转发到对应server进行
                        MqttApplicationMessage applicationMessage = new MqttApplicationMessageBuilder()
                           .WithTopic($"server/from/{server.Identifier}/proxy")
                           .WithPayload(JsonSerializer.Serialize(new ProxyModel
                           {
                               topic = $"client/to/{executor.ClientId}/onjob",
                               data = JsonSerializer.Serialize(new OnTaskRequest
                               {
                                   Job = new TodoTask
                                   {
                                       TaskId = task.TaskId,
                                       Name = job.Name,
                                       GroupName = job.Appid,
                                       TimeExpression = job.TimeConf,
                                       TimeType = job.TimeType,
                                       MaxAttempt = job.AttemptMax,
                                       AttemptInterval = job.AttemptInterval,
                                       Content = job.Handler,
                                       Description = job.Description,
                                       ExecuteMode = job.ScheduleMode,
                                       JobId = job.Id,
                                       JobParams = job.HandleParams
                                   }
                               })
                           }
                          ))
                           .Build();

                        _ = await server.PublishAsync(applicationMessage);
                    }

                    taskService.UpdateTaskFlag(task.TaskId, (int)TaskLogStatus.Process);
                }

                return (true, "指令发送成功");
            }
            catch
            {
                jobService.UpdateParallelCount(job.Id, -1);
                throw;
            }
        }
    }
}
