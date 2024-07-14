﻿// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

namespace OpenTask.WebApi.Models
{
    public class GetJobListRequest
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 15;

        public string? name { get; set; }
    }
}
