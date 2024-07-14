// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using MediatR;
using OpenTask.Application.Base.Commands;
using OpenTask.Application.Core.Interface;
using OpenTask.Domain.TaskInfos;

namespace OpenTask.Application.TaskInfos
{
    public class TaskInfosHandler :
        IRequestHandler<ListTaskInfosRequest, ListTaskInfosResponse>,
        ICommandHandler<AddTaskRequest, AddTaskResponse>,
        ICommandHandler<DeleteTaskRequest, DeleteTaskResponse>,
        ICommandHandler<ExcuteOnceRequest, ExcuteOnceResponse>,
        ICommandHandler<GetNext5TriggertimesRequest, GetNext5TriggertimesResponse>,
        ICommandHandler<SwitchTaskStatusRequest, SwitchTaskStatusResponse>
    {
        private readonly ITaskInfoRepository taskInfoRepository;
        private readonly ITaskDispatcher excuteJobHandler;
        private readonly ITaskServer mqttServer;

        public TaskInfosHandler(ITaskInfoRepository taskInfoRepository, ITaskDispatcher excuteJobHandler, ITaskServer mqttServer)
        {
            this.excuteJobHandler = excuteJobHandler;
            this.taskInfoRepository = taskInfoRepository;
            this.mqttServer = mqttServer;
        }

        public Task<ListTaskInfosResponse> Handle(ListTaskInfosRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<TaskInfo> list = taskInfoRepository.ListJobs(request.PageNumber, request.PageSize, request.Name, out long count);

            return Task.FromResult(new ListTaskInfosResponse
            {
                Count = count,
                Rows = list
            });
        }

        public Task<AddTaskResponse> Handle(AddTaskRequest request, CancellationToken cancellationToken)
        {
            _ = taskInfoRepository.AddJob(request.Task);
            return Task.FromResult(new AddTaskResponse()
            {
            });
        }

        public Task<DeleteTaskResponse> Handle(DeleteTaskRequest request, CancellationToken cancellationToken)
        {
            taskInfoRepository.DelJob(request.TaskId);
            return Task.FromResult(new DeleteTaskResponse());
        }

        public Task<SwitchTaskStatusResponse> Handle(SwitchTaskStatusRequest request, CancellationToken cancellationToken)
        {
            _ = taskInfoRepository.SwitchEnabledStatus(request.TaskId, request.Enabled);
            return Task.FromResult(new SwitchTaskStatusResponse());
        }

        public async Task<ExcuteOnceResponse> Handle(ExcuteOnceRequest request, CancellationToken cancellationToken)
        {
            TaskInfo job = getTask(request.TaskId);
            (_, _) = await excuteJobHandler.DispatchAsync(mqttServer, job);

            return new ExcuteOnceResponse
            {

            };
        }

        private TaskInfo getTask(long taskId)
        {
            TaskInfo job = taskInfoRepository.GetJob(taskId);
            return job ?? throw new Exception("任务不存在");
        }


        public Task<GetNext5TriggertimesResponse> Handle(GetNext5TriggertimesRequest request, CancellationToken cancellationToken)
        {
            TaskInfo job = getTask(request.TaskId);
            IEnumerable<DateTime> res = job.GetNextOccurrence(5);

            return Task.FromResult(new GetNext5TriggertimesResponse
            {
                NextTriggers = res
            });
        }
    }
}
