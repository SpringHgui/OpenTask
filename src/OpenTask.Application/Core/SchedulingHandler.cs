// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using OpenTask.Application.Core.Interface;
using OpenTask.Domain.TaskInfos;
using OpenTask.Utility.Extensions;
using OpenTask.Utility.Helpers;
using WheelTimer;
using WheelTimer.Utilities;

namespace OpenTask.Application.Core
{
    public class SchedulingHandler : ITaskSchedulingHandler
    {
        private HashedWheelTimer? hashedWheelTimer;
        private readonly ILogger<SchedulingHandler> logger;
        private readonly IServiceProvider service;
        private readonly System.Timers.Timer timer;
        private readonly ITaskDispatcher excuteJobHandler;
        private ITaskServer taskServer = null!;
        private bool stoped = false;
        private bool stared = false;

        private readonly object locker = new();

        public SchedulingHandler(ITaskDispatcher excuteJobHandler,
            IServiceProvider service, ILogger<SchedulingHandler> logger)
        {
            this.excuteJobHandler = excuteJobHandler;
            this.service = service;
            this.logger = logger;

            timer = new System.Timers.Timer()
            {
                Interval = 15000,// 15秒加载一次
            };

            timer.Elapsed += Timer_Elapsed;
        }

        public Task Start(ITaskServer taskServer, CancellationToken stoppingToken)
        {
            if (stared)
            {
                throw new InvalidOperationException();
            }

            lock (locker)
            {
                if (!stared)
                {
                    stared = true;
                }
            }

            this.taskServer = taskServer;

            hashedWheelTimer = new HashedWheelTimer();

            loadTaskInfos();
            timer.Start();

            return Task.CompletedTask;
        }

        public void StartDispatch()
        {
            logger.LogInformation($"StartDispatch {stoped}");
            if (!stoped)
            {
                return;
            }

            lock (locker)
            {
                stoped = false;
                logger.LogInformation("重建时间轮");
                hashedWheelTimer = new HashedWheelTimer();

                // 先直接加载一次，避免任务与预定执行时间偏差太大
                loadTaskInfos();
                timer.Start();
            }
        }

        public void StopDispatch()
        {
            logger.LogInformation($"StopDispatch {stoped}");
            if (stoped)
            {
                return;
            }

            lock (locker)
            {
                stoped = true;

                timer.Stop();

                if (hashedWheelTimer != null)
                {
                    logger.LogInformation("停止时间轮");
                    // 清空时间轮
                    hashedWheelTimer.StopAsync().Wait();
                    hashedWheelTimer = null;
                }
            }
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            loadTaskInfos();
        }

        private void loadTaskInfos()
        {
            try
            {
                if (stoped)
                {
                    return;
                }

                using IServiceScope scope = service.CreateScope();

                ITaskInfoRepository jobService = scope.ServiceProvider.GetRequiredService<ITaskInfoRepository>();

                IEnumerable<TaskInfo> jobs = jobService.GetNextJob(taskServer.Identifier, DateTime.Now.AddSeconds(15).ToTimestamp());

                logger.LogDebug($"[拉取待执行任务]: {taskServer.Identifier} => count={jobs.Count()}");

                foreach (TaskInfo item in jobs)
                {
                    if (!item.Enabled)
                    {
                        continue;
                    }

                    pushToTimer(item);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ReloadScheduler Error");
            }
        }

        private void pushToTimer(TaskInfo jobMolde)
        {
            if (hashedWheelTimer == null)
            {
                logger.LogError($"hashedWheelTimer is null");
                return;
            }

            TimeCrontab.Crontab crontab = CrontabUtility.Parse(jobMolde.TimeConf);
            DateTime nextOccurrence = crontab.GetNextOccurrence(DateTime.Now);

            TimeSpan delay = nextOccurrence - DateTime.Now;
            logger.LogInformation($"[插入时间轮]:{jobMolde.Id} {jobMolde.Name} {delay.TotalSeconds}s后执行");

            _ = hashedWheelTimer.NewTimeout(new ActionTimerTask((job) =>
            {
                try
                {
                    if (stoped)
                    {
                        throw new Exception("Scheduling has stoped");
                    }

                    logger.LogInformation($"[任务执行]: {jobMolde.Name} {jobMolde.Description}");

                    // 查询当前任务状态，如果未启用，则不再执行，也停止下个周期的调度
                    using IServiceScope scope = service.CreateScope();
                    ITaskInfoRepository jobService = scope.ServiceProvider.GetRequiredService<ITaskInfoRepository>();

                    // 重新获取，避免在等待执行期间数据库中的数据被修改导致不一致
                    if (jobMolde == null)
                    {
                        logger.LogError($"任务不存：{jobMolde.Id}");
                        return;
                    }

                    // 
                    if (!jobMolde.Enabled)
                    {
                        logger.LogError($"任务已禁用：{jobMolde.Id}");
                        return;
                    }

                    try
                    {
                        (bool Success, string Message) = excuteJobHandler.DispatchAsync(taskServer, jobMolde).GetAwaiter().GetResult();

                        // 继续插入下个周期
                        DateTime nextOccurrence = crontab.GetNextOccurrence(DateTime.Now);
                        jobService.UpdateNextTriggerTime(jobMolde.Id, DateTime.Now.ToTimestamp(), nextOccurrence.ToTimestamp());

                        logger.LogInformation($"[下发调度任务结果]：{Message}");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "调度失败");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "任务调度失败");
                }
            }), delay);
        }
    }
}
