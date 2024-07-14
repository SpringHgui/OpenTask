// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H


namespace OpenTask.Core.Models
{
    public class OnTaskRequest : BaseMassageAgrs
    {
        public TodoTask? Job { get; set; }

        public bool Success { get; set; }

        public string? ErrMsg { get; set; }
    }
}
