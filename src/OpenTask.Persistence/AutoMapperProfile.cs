// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using AutoMapper;
using OpenTask.Domain.Lockers;
using OpenTask.Domain.Servers;
using OpenTask.Domain.TaskInfos;
using OpenTask.Domain.TaskLogs;
using OpenTask.Domain.Users;
using OpenTask.Persistence.Entitys;
using OpenTask.Persistence.Models;

namespace OpenTask.Persistence
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            _ = CreateMap<TaskLog, OtTaskLog>().ReverseMap();
            _ = CreateMap<TaskInfo, OtTaskInfo>().ReverseMap();
            _ = CreateMap<User, OtUser>().ReverseMap();
            _ = CreateMap<OpenTaskServer, OtServer>().ReverseMap();
            _ = CreateMap<Locker, OtLocker>().ReverseMap();

            _ = CreateMap<TaskInfoBaseStatisticsDbModel, TaskInfoBaseStatistics>();
            _ = CreateMap<TaskLogBaseStatisticsDbModel, TaskLogBaseStatistics>();
            _ = CreateMap<TopTaskLogDbModel, TopTaskLog>();
            _ = CreateMap<TaskLogDayTrendDbModel, TaskLogDayTrend>();
        }
    }
}
