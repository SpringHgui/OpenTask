// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

namespace OpenTask.Application.Core.Models
{
    [Obsolete("使用Server替代", true)]
    public class Server
    {
        public required string Endpoint { get; set; }

        public required string Guid { get; set; }

        /// <summary>
        /// 0~16383
        /// </summary>
        public int SlotStart { get; set; }

        public int SlotEnd { get; set; }

        public long Id { get; set; }

        public DateTime HeartAt { get; set; }
    }
}
