// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using MediatR;
using System.ComponentModel.DataAnnotations;

namespace OpenTask.Application.TaskLogs
{
    public class ListTopTaskLogRequest : IRequest<ListTopTaskLogResponse>
    {
        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        [Range(1, 30)]
        public int Count { get; set; }
    }
}
