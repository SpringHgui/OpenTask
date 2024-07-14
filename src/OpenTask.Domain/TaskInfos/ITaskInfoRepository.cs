// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using OpenTask.Domain.Base.Repositorys;

namespace OpenTask.Domain.TaskInfos
{
    public interface ITaskInfoRepository : IRepository<TaskInfo>
    {
        TaskInfo AddJob(TaskInfo model);

        void DelJob(long jobId);

        TaskInfoBaseStatistics GetBaseStatistics();

        TaskInfo GetJob(long jobId);

        IEnumerable<TaskInfo> GetNextJob(string guid, long timeStamp);

        IEnumerable<TaskInfo> ListJobs(int pageNumber, int pageSize, string? name, out long count);

        bool SwitchEnabledStatus(long jobId, bool enbaled);

        bool UpdateNextTriggerTime(long id, long last, long next);

        void UpdateParallelCount(long jobId, int v);
    }
}
