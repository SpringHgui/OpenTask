// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using AutoMapper;
using Dapper;
using Dommel;
using OpenTask.Domain.TaskInfos;
using OpenTask.Persistence.Entitys;
using OpenTask.Persistence.Extensions;
using OpenTask.Persistence.Models;

namespace OpenTask.Persistence.Repositorys
{
    [Inject]
    public class TaskInfoRepository : ITaskInfoRepository
    {
        private readonly DapperdbContext dbContext;
        private readonly IMapper mapper;

        public TaskInfoRepository(DapperdbContext dbContext, IMapper mapper)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public TaskInfo AddJob(TaskInfo model)
        {
            using var conn = dbContext.CreateConnection();
            var dbModel = mapper.Map<OtTaskInfo>(model);
            dbModel.Enabled = true;

            object id = conn.Insert(dbModel);
            dbModel.Id = Convert.ToInt64(id);
            dbModel.Slot = NullFX.CRC.Crc16.ComputeChecksum(NullFX.CRC.Crc16Algorithm.Standard, BitConverter.GetBytes(model.Id)) & 16383;

            conn.Update<OtTaskInfo>(dbModel);
            return mapper.Map<TaskInfo>(dbModel);
        }

        public void DelJob(long jobId)
        {
            using var conn = dbContext.CreateConnection();
            _ = conn.Delete(new OtTaskInfo { Id = jobId });
        }

        public TaskInfo? GetJob(long jobId)
        {
            using var conn = dbContext.CreateConnection();
            return mapper.Map<TaskInfo>(conn.FirstOrDefault((OtTaskInfo x) => x.Id == jobId));
        }

        public IEnumerable<TaskInfo> ListJobs(int pageNumber, int pageSize, string? name, out long count)
        {
            using var conn = dbContext.CreateConnection();
            if (name == null)
            {
                count = conn.Count<OtTaskInfo>();

                return conn
                    .From<OtTaskInfo>(sql => sql.Page(pageNumber, pageSize).Select())
                    .Select(mapper.Map<TaskInfo>);
            }
            else
            {
                count = conn.Count<OtTaskInfo>(x => x.Name.Contains(name));
                return conn
                  .From<OtTaskInfo>(sql => sql.Where(x => x.Name.Contains(name)).Page(pageNumber, pageSize).Select())
                  .Select(mapper.Map<TaskInfo>);
            }
        }

        public IEnumerable<TaskInfo> GetNextJob(string serverId, long timestamp)
        {
            string sql = """
                SELECT t.*
                FROM ot_task_info t
                JOIN ot_server s ON t.slot >= s.slot_from AND t.slot <= s.slot_end
                WHERE s.server_id = @serverId and t.trigger_next_time <=@timestamp and t.enabled=1
                """;
            using var conn = dbContext.CreateConnection();
            IEnumerable<TaskInfo> list = conn.Query<OtTaskInfo>(sql, new { serverId, timestamp })
                .Select(mapper.Map<TaskInfo>);
            return list;
        }

        public void UpdateParallelCount(long jobid, int count)
        {
            //// 这里并发时，可能出现数量维护不一致
            //var job = GetJob(jobid);
            ////job.ThreadCount += count;

            //dbContext.ScJobs.Update(job);
            //dbContext.SaveChangesAsync();
        }

        public bool UpdateNextTriggerTime(long id, long last, long next)
        {
            using var conn = dbContext.CreateConnection();
            var sql = "update ot_task_info set trigger_next_time=@trigger_next_time,trigger_last_time=@trigger_last_time where id=@id";
            return conn.Execute(sql, new { id, trigger_last_time = last, trigger_next_time = next }) == 1;
        }

        public bool SwitchEnabledStatus(long id, bool enabled)
        {
            using var conn = dbContext.CreateConnection();
            var sql = "update ot_task_info set enabled=@enabled where id=@id";
            return conn.Execute(sql, new { id, enabled }) == 1;
        }

        public TaskInfoBaseStatistics GetBaseStatistics()
        {
            using var conn = dbContext.CreateConnection();
            var sql = "SELECT COUNT(*) total,COUNT(case when enabled > 0 then 1 end ) enabled FROM ot_task_info";
            return mapper.Map<TaskInfoBaseStatistics>(conn.QuerySingle<TaskInfoBaseStatisticsDbModel>(sql));
        }
    }
}