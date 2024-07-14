// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using OpenTask.Domain.Base.Repositorys;

namespace OpenTask.Domain.TaskLogs
{
    public interface ITaskLogRepository : IRepository<TaskLog>
    {
        TaskLog AddTask(TaskLog task);

        TaskLogBaseStatistics GetBaseStatistics();

        IEnumerable<TaskLogDayTrend> GetDayTrend(DateTime start, DateTime end);

        TaskLog? GetTaskById(long taskId);

        IEnumerable<TaskLog> LisTasks(int pageNumber, int pageSize, long? jobId, DateTime? startTime, DateTime? endTime, out long count);

        void Update(TaskLog task);

        void UpdateTaskFlag(long taskId, int process);

        IEnumerable<TopTaskLog> GetTopTaskLog(DateTime start, DateTime end, int count);
    }
}
