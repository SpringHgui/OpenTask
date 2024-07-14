// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

namespace OpenTask.Domain.TaskLogs
{
    public class TaskLog
    {
        public long Id { get; set; }

        public long TaskId { get; set; }

        public DateTime? HandleStart { get; set; }

        public DateTime? HandleEnd { get; set; }

        public string HandleResult { get; set; } = "";

        public sbyte HandleStatus { get; set; }

        public string HandleClient { get; set; } = "";

        public int AlarmStatus { get; set; }
    }
}
