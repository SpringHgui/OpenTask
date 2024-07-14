// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using MediatR;
using OpenTask.Domain.TaskLogs;

namespace OpenTask.Application.TaskLogs
{
    public class LisTaskLogsRequestHandler :
        IRequestHandler<LisTaskLogsRequest, LisTaskLogsResponse>,
        IRequestHandler<TaskLogsDayTrendRequest, TaskLogsDayTrendResponse>,
        IRequestHandler<ListTopTaskLogRequest, ListTopTaskLogResponse>
    {
        private readonly ITaskLogRepository taskLogRepository;

        public LisTaskLogsRequestHandler(ITaskLogRepository tasklogRepository)
        {
            taskLogRepository = tasklogRepository;
        }

        public Task<LisTaskLogsResponse> Handle(LisTaskLogsRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<TaskLog> list = taskLogRepository.LisTasks(
                request.PageNumber, request.PageSize, request.TaskId, request.StartTime, request.EndTime, out long count);

            return Task.FromResult(new LisTaskLogsResponse
            {
                Count = count,
                Rows = list
            });
        }

        public Task<TaskLogsDayTrendResponse> Handle(TaskLogsDayTrendRequest request, CancellationToken cancellationToken)
        {
            var res = taskLogRepository.GetDayTrend(request.Start, request.End);

            return Task.FromResult(new TaskLogsDayTrendResponse
            {
                data = res
            });
        }

        public Task<ListTopTaskLogResponse> Handle(ListTopTaskLogRequest request, CancellationToken cancellationToken)
        {
            var res = taskLogRepository.GetTopTaskLog(request.Start, request.End, request.Count);

            return Task.FromResult(new ListTopTaskLogResponse
            {
                Data = res
            });
        }
    }
}
