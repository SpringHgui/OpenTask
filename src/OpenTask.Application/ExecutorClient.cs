// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using System.Text.Json.Serialization;

namespace OpenTask.Application
{
    public class ExecutorClient
    {
        [JsonIgnore]
        public string? ServerId { get; set; }

        public required string GroupName { get; set; }

        public string? ConnectionId { get; set; }

        public IEnumerable<string>? Handelrs { get; set; }

        public DateTime StartTime { get; set; }

        public required string ClientId { get; set; }
    }
}
