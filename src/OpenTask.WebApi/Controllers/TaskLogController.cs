// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenTask.Application.Base;
using OpenTask.Application.TaskLogs;

namespace OpenTask.WebApi.Controllers
{
    public class TaskLogController : BaseController
    {
        public TaskLogController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet]
        public async Task<BaseResponse<LisTaskLogsResponse>> ListLogs([FromQuery] LisTaskLogsRequest tasksCommand)
        {
            return await RequestAsync<LisTaskLogsRequest, LisTaskLogsResponse>(tasksCommand);
        }
    }
}
