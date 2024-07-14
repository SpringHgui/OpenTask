// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using AutoMapper;
using Dapper;
using Dommel;
using OpenTask.Domain.TaskLogs;
using OpenTask.Persistence.Entitys;
using OpenTask.Persistence.Extensions;
using OpenTask.Persistence.Models;
using System.Text;
using TaskLog = OpenTask.Domain.TaskLogs.TaskLog;

namespace OpenTask.Persistence.Repositorys
{
    [Inject]
    public class TaskLogRepository : ITaskLogRepository
    {
        private readonly DapperdbContext dbContext;
        private readonly IMapper mapper;

        public TaskLogRepository(DapperdbContext dbContext, IMapper mapper)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public TaskLog AddTask(TaskLog task)
        {
            using var conn = dbContext.CreateConnection();
            object id = conn.Insert<OtTaskLog>(mapper.Map<OtTaskLog>(task));
            task.TaskId = Convert.ToInt64(id);
            return mapper.Map<TaskLog>(task);
        }

        public TaskLogBaseStatistics GetBaseStatistics()
        {
            using var conn = dbContext.CreateConnection();
            var sql = "SELECT COUNT(*) total,COUNT(case when handle_status = 2 then 1 end ) success,COUNT(case when handle_status = 3 then 1 end ) fail,COUNT(case when handle_status = 1 then 1 end ) process FROM ot_task_log";
            return mapper.Map<TaskLogBaseStatistics>(conn.QuerySingle<TaskLogBaseStatisticsDbModel>(sql));
        }

        public IEnumerable<TaskLogDayTrend> GetDayTrend(DateTime start, DateTime end)
        {
            using var conn = dbContext.CreateConnection();
            var sql = "SELECT count(*) total,COUNT(case when handle_status = 2 then 1 end ) success,date_format(handle_start, '%Y-%m-%d') `day` FROM ot_task_log WHERE handle_start>=@start AND handle_start<=@end group by date_format(handle_start, '%Y-%m-%d') order by `day`";
            return conn.Query<TaskLogDayTrendDbModel>(sql, new { start = start.ToLocalTime(), end = end.ToLocalTime() }).Select(mapper.Map<TaskLogDayTrend>);
        }

        public IEnumerable<TopTaskLog> GetTopTaskLog(DateTime start, DateTime end, int count)
        {
            using var conn = dbContext.CreateConnection();
            var sql = "SELECT task_id,B.`name`,description,count(*) count FROM ot_task_log A LEFT JOIN ot_task_info B ON A.task_id=B.id WHERE handle_start>=@start AND handle_start<=@end GROUP BY task_id order by count desc limit @count";
            return conn.Query<TopTaskLogDbModel>(sql, new { start = start.ToLocalTime(), end = end.ToLocalTime(), count }).Select(mapper.Map<TopTaskLog>);
        }

        public TaskLog? GetTaskById(long taskId)
        {
            using var conn = dbContext.CreateConnection();
            var log = conn.FirstOrDefault<OtTaskLog>(x => x.Id == taskId);
            return mapper.Map<TaskLog>(log);
        }

        public IEnumerable<TaskLog> LisTasks(int pageNumber, int pageSize, long? jobId, DateTime? startTime, DateTime? endTime, out long count)
        {
            using var conn = dbContext.CreateConnection();
            var where = new StringBuilder("WHERE 1=1");
            if (jobId.HasValue)
            {
                _ = where.Append(" AND task_id=@task_id");
            }

            if (startTime.HasValue)
            {
                startTime = startTime.Value.ToLocalTime();
                _ = where.Append(" AND handle_start>=@handle_start");
            }

            if (endTime.HasValue)
            {
                endTime = endTime.Value.ToLocalTime();
                _ = where.Append(" AND handle_start<=@end_time");
            }

            var param = new { task_id = jobId, handle_start = startTime, end_time = endTime };

            count = conn.ExecuteScalar<long>($"select count(*) count from ot_task_log {where}", param);
            return conn.Query<OtTaskLog>($"select * from ot_task_log {where} order by id desc limit {pageSize * (pageNumber - 1)},{pageSize}", param).Select(mapper.Map<TaskLog>);
        }

        public void Update(TaskLog task)
        {
            using var conn = dbContext.CreateConnection();
            _ = conn.Update<OtTaskLog>(mapper.Map<OtTaskLog>(task));
        }

        public void UpdateTaskFlag(long taskId, int send)
        {
            using var conn = dbContext.CreateConnection();
            OtTaskLog? task = conn.FirstOrDefault<OtTaskLog>(x => x.Id == taskId);
            if (task == null)
            {
                throw new Exception("任务实例不存在");
            }

            task.HandleStatus = (sbyte)send;
            _ = conn.Update(task);
        }
    }
}
