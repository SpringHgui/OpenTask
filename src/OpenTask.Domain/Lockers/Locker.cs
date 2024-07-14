// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

namespace OpenTask.Domain.Lockers
{
    public class Locker
    {
        public string Resource { get; set; } = null!;

        public int Version { get; set; }

        public bool Locked { get; set; }

        public DateTime LockedAt { get; set; }

        public string LockedBy { get; set; } = null!;

        /// <summary>
        /// 默认超过多久视为无效
        /// </summary>
        public bool IsExpired(int secd)
        {
            return (DateTime.Now - LockedAt).TotalSeconds > secd;
        }
    }
}
