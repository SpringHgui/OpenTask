// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenTask.Application.Base;
using OpenTask.Application.Dashboard;
using OpenTask.Application.TaskLogs;

namespace OpenTask.WebApi.Controllers
{
    public class DashboardController : BaseController
    {
        public DashboardController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<BaseResponse<StatisticsResponse>> Statistics()
        {
            return await RequestAsync<StatisticsRequest, StatisticsResponse>(new StatisticsRequest());
        }

        [HttpGet]
        public async Task<BaseResponse<TaskLogsDayTrendResponse>> TaskLogsDayTrend([FromQuery] TaskLogsDayTrendRequest request)
        {
            return await RequestAsync<TaskLogsDayTrendRequest, TaskLogsDayTrendResponse>(request);
        }

        [HttpGet]
        public async Task<BaseResponse<ListTopTaskLogResponse>> TopTaskLog([FromQuery] ListTopTaskLogRequest request)
        {
            return await RequestAsync<ListTopTaskLogRequest, ListTopTaskLogResponse>(request);
        }
    }
}
