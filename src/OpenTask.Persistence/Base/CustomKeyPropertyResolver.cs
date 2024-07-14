// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Dommel;

namespace OpenTask.Persistence.Base
{
    public class CustomKeyPropertyResolver : IKeyPropertyResolver
    {
        public ColumnPropertyInfo[] ResolveKeyProperties(Type type)
        {
            return new[] { new ColumnPropertyInfo(type.GetProperties().Single(p => p.Name == $"{type.Name}Id"), isKey: true) };
        }
    }
}
