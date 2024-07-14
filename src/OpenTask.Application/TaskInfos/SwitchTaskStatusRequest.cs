﻿// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using OpenTask.Application.Base.Commands;

namespace OpenTask.Application.TaskInfos
{
    public class SwitchTaskStatusRequest : ICommand<SwitchTaskStatusResponse>
    {
        public long TaskId { get; set; }

        public bool Enabled { get; set; }
    }
}
