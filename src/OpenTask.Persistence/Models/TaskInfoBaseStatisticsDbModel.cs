// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using System.ComponentModel.DataAnnotations.Schema;

namespace OpenTask.Persistence.Models
{
    public class TaskInfoBaseStatisticsDbModel
    {
        [Column("enabled")]
        public long Enabled { get; set; }

        [Column("total")]
        public long Total { get; set; }
    }
}
