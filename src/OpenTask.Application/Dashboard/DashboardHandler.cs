// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using OpenTask.Application.Base.Commands;
using OpenTask.Application.Core.Interface;
using OpenTask.Domain.TaskInfos;
using OpenTask.Domain.TaskLogs;

namespace OpenTask.Application.Dashboard
{
    public class DashboardHandler :
         ICommandHandler<StatisticsRequest, StatisticsResponse>
    {
        private readonly ITaskInfoRepository taskInfoRepository;
        private readonly ITaskServer mqttServer;
        private readonly ITaskLogRepository taskLogRepository;

        public DashboardHandler(ITaskInfoRepository taskInfoRepository, ITaskLogRepository taskLogRepository, ITaskServer mqttServer)
        {
            this.taskLogRepository = taskLogRepository;
            this.taskInfoRepository = taskInfoRepository;
            this.taskInfoRepository = taskInfoRepository;
            this.mqttServer = mqttServer;
        }

        public Task<StatisticsResponse> Handle(StatisticsRequest request, CancellationToken cancellationToken)
        {
            var server = mqttServer.Discovery.FindAllOnlineServer();
            var taskInfo = taskInfoRepository.GetBaseStatistics();
            var tasklog = taskLogRepository.GetBaseStatistics();
            var clients = mqttServer.GetAllClientsOnline();

            return Task.FromResult(new StatisticsResponse
            {
                Servers = new StatisticsResponse.StatisticInfos
                {
                    Count = server.Count()
                },
                TaskInfos = new StatisticsResponse.StatisticInfos
                {
                    Count = taskInfo.Total
                },
                TaskLogs = new StatisticsResponse.StatisticInfos
                {
                    Count = tasklog.Total
                },
                Workers = new StatisticsResponse.StatisticInfos
                {
                    Count = clients.Count()
                }
            });
        }
    }
}
