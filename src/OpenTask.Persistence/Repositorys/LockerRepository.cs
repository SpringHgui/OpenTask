// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using AutoMapper;
using Dapper;
using Dommel;
using MySql.Data.MySqlClient;
using OpenTask.Domain.Lockers;
using OpenTask.Persistence.Entitys;
using OpenTask.Persistence.Extensions;
using OpenTask.Persistence.Models;

namespace OpenTask.Persistence.Repositorys
{
    [Inject]
    public class LockerRepository : ILockerRepository
    {
        private readonly DapperdbContext dbContext;
        private readonly IMapper mapper;

        public LockerRepository(DapperdbContext dbContext, IMapper mapper)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public Locker FindByKey(string resource)
        {
            using var conn = dbContext.CreateConnection();
            return mapper.Map<Locker>(conn.FirstOrDefault<OtLocker>(x => x.Resource == resource));
        }

        public bool NewLocker(string key, string lockerby, out Locker? locker)
        {
            OtLocker item = new()
            {
                Resource = key,
                LockedBy = lockerby,
                LockedAt = DateTime.Now,
                Version = 0
            };

            using var conn = dbContext.CreateConnection();
            string sql = "insert into `ot_locker` (`resource`,`version`, `locked_by`, `locked_at`) values (@Resource, @Version, @LockedBy, @LockedAt);";
            try
            {
                int count = conn.Execute(sql, item);
                locker = mapper.Map<Locker>(item);
                return true;
            }
            catch (MySqlException)
            {
                locker = null;
                return false;
            }
        }

        public bool ReleaseLocker(string key, string lockerby)
        {
            using var conn = dbContext.CreateConnection();
            string sql = "delete from `ot_locker` where locked_by=@lockerby and resource=@key";
            return conn.Execute(sql, new { key, lockerby }) == 1;
        }

        public bool UpdateLocker(string key, string lockerby, int version)
        {
            using var conn = dbContext.CreateConnection();
            string sql = "update `ot_locker` set version=version+1,locked_at=@locked_at,locked_by=@lockerby where version=@version and resource=@key";
            return conn.Execute(sql, new { key, version, locked_at = DateTime.Now, lockerby }) == 1;
        }

        public bool ReEnterLocker(string key, string lockerby, int version)
        {
            using var conn = dbContext.CreateConnection();
            string sql = "update `ot_locker` set version=version+1,locked_at=@locked_at where version=@version and resource=@key and locked_by=@lockerby";
            return conn.Execute(sql, new { key, version, locked_at = DateTime.Now, lockerby }) == 1;
        }
    }
}
