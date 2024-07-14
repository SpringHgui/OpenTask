// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using System.ComponentModel.DataAnnotations.Schema;

namespace OpenTask.Persistence.Models
{
    public class TopTaskLogDbModel
    {
        [Column("task_id")]
        public long TaskId { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public long Count { get; set; }
    }
}
