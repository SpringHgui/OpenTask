// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using System.ComponentModel.DataAnnotations.Schema;

namespace OpenTask.Persistence.Models
{
    public class TaskLogBaseStatisticsDbModel
    {
        [Column("total")]
        public long Total { get; set; }

        [Column("success")]
        public long Success { get; set; }

        [Column("fail")]
        public long Fail { get; set; }

        [Column("process")]
        public long Process { get; set; }
    }
}
