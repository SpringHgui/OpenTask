// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

namespace OpenTask.Application.Dashboard
{
    public class StatisticsResponse
    {
        public required StatisticInfos TaskInfos { get; set; }

        public required StatisticInfos TaskLogs { get; set; }

        public required StatisticInfos Servers { get; set; }

        public required StatisticInfos Workers { get; set; }

        public class StatisticInfos
        {
            public long Count { get; set; }
        }
    }
}
