// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

namespace OpenTask.Domain.TaskLogs
{
    public class TaskLogBaseStatistics
    {
        public long Total { get; set; }

        public long Success { get; set; }

        public long Fail { get; set; }

        public long Process { get; set; }
    }
}
