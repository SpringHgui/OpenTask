// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

namespace OpenTask.Domain.TaskInfos
{
    public class TaskInfoBaseStatistics
    {
        public long Enabled { get; set; }

        public long Total { get; set; }

        public long Disabled => Total - Enabled;
    }
}
