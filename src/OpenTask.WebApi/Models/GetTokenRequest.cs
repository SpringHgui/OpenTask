// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

namespace OpenTask.WebApi.Models
{
    public class GetTokenRequest
    {
        public required string UserName { get; set; }

        public required string PassWord { get; set; }
    }
}
