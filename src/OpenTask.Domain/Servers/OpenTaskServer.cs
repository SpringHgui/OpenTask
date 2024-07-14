// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

namespace OpenTask.Domain.Servers
{
    public class OpenTaskServer
    {
        public long Id { get; set; }

        public string ServerId { get; set; } = null!;

        public string EndPoint { get; set; } = null!;

        public DateTime HeartAt { get; set; }

        public int SlotFrom { get; set; }

        public int SlotEnd { get; set; }
    }
}
