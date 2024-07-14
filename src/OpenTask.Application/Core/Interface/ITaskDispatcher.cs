// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using OpenTask.Domain.TaskInfos;

namespace OpenTask.Application.Core.Interface
{
    public interface ITaskDispatcher
    {
        Task<(bool Success, string Message)> DispatchAsync(ITaskServer server, TaskInfo job);
    }
}
