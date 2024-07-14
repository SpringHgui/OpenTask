// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using MediatR;
using OpenTask.Application.Base;

namespace OpenTask.Application.TaskLogs
{
    public class LisTaskLogsRequest : PageQuery, IRequest<LisTaskLogsResponse>
    {
        public long? TaskId { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }
    }
}
