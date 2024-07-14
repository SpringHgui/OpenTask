// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using MediatR;

namespace OpenTask.Application.TaskLogs
{
    /// <summary>
    /// 调度日志 日走势
    /// </summary>
    public class TaskLogsDayTrendRequest : IRequest<TaskLogsDayTrendResponse>
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}
