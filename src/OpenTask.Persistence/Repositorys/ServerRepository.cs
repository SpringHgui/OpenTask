// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using AutoMapper;
using Dommel;
using OpenTask.Domain.Servers;
using OpenTask.Persistence.Entitys;
using OpenTask.Persistence.Extensions;
using OpenTask.Persistence.Models;

namespace OpenTask.Persistence.Repositorys
{
    [Inject]
    public class ServerRepository : IServerRepository
    {
        private readonly DapperdbContext dbContext;
        private readonly IMapper mapper;

        public ServerRepository(DapperdbContext dbContext, IMapper mapper)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public IEnumerable<OpenTaskServer> GetServerOnline(int heart)
        {
            using var conn = dbContext.CreateConnection();
            DateTime time = DateTime.Now.AddSeconds(-heart);
            return conn.Select<OtServer>(x => x.HeartAt > time).Select(mapper.Map<OpenTaskServer>);
        }

        public void RegisterOrUpdate(OpenTaskServer mqttNode)
        {
            using var conn = dbContext.CreateConnection();
            IEnumerable<OtServer> olds = conn.Select<OtServer>(x => x.ServerId == mqttNode.ServerId);
            if (!olds.Any())
            {
                _ = conn.Insert<OtServer>(mapper.Map<OtServer>(mqttNode));
            }
            else
            {
                OtServer old = olds.FirstOrDefault()!;
                old.HeartAt = mqttNode.HeartAt;
                _ = conn.Update<OtServer>(old);
            }
        }

        public void UpdateSlot(IEnumerable<OpenTaskServer> nodes)
        {
            using var conn = dbContext.CreateConnection();
            foreach (OpenTaskServer item in nodes)
            {
                OtServer server = mapper.Map<OtServer>(item);

                _ = conn.Update<OtServer>(server);
            }
        }
    }
}
