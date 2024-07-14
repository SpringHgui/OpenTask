// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

namespace OpenTask.Domain.Base.Types
{
    public class TeamName
    {
        public TeamName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            if (name.Length >= 20)
            {
                throw new ArgumentOutOfRangeException(nameof(name), name.Length, "超长");
            }

            Name = name;
        }

        public string Name { get; private set; }
    }
}
