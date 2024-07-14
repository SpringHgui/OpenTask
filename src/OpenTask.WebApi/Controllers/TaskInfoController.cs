// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenTask.Application.Base;
using OpenTask.Application.TaskInfos;

namespace OpenTask.WebApi.Controllers
{
    public class TaskInfoController : BaseController
    {
        public TaskInfoController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<BaseResponse<ListTaskInfosResponse>> ListTaskInfos([FromQuery] ListTaskInfosRequest request)
        {
            return await RequestAsync<ListTaskInfosRequest, ListTaskInfosResponse>(request);
        }

        [HttpPost]
        public async Task<BaseResponse<AddTaskResponse>> AddTaskInfo(AddTaskRequest request)
        {
            return await RequestAsync<AddTaskRequest, AddTaskResponse>(request);
        }

        [HttpDelete]
        public async Task<BaseResponse<DeleteTaskResponse>> DeleteTask(DeleteTaskRequest request)
        {
            return await RequestAsync<DeleteTaskRequest, DeleteTaskResponse>(request);
        }

        [HttpPost]
        public async Task<BaseResponse<SwitchTaskStatusResponse>> SwitchTaskStatus(SwitchTaskStatusRequest request)
        {
            return await RequestAsync<SwitchTaskStatusRequest, SwitchTaskStatusResponse>(request);
        }

        [HttpPost]
        public async Task<BaseResponse<ExcuteOnceResponse>> ExcuteOnce(ExcuteOnceRequest request)
        {
            return await RequestAsync<ExcuteOnceRequest, ExcuteOnceResponse>(request);
        }

        /// <summary>
        /// 获取最近
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<BaseResponse<GetNext5TriggertimesResponse>> GetNext5Tiggertimes([FromQuery] GetNext5TriggertimesRequest request)
        {
            return await RequestAsync<GetNext5TriggertimesRequest, GetNext5TriggertimesResponse>(request);
        }
    }
}
