// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Dommel;
using System.Reflection;
using System.Text.Json;

namespace OpenTask.Persistence.Base
{
    public class CustomColumnNameResolver : IColumnNameResolver
    {
        public string ResolveColumnName(PropertyInfo propertyInfo)
        {
            // Every column has prefix 'fld' and is uppercase.
            JsonNamingPolicy namingPolicy = JsonNamingPolicy.SnakeCaseLower;
            string snakeCaseString = namingPolicy.ConvertName(propertyInfo.Name);

            return snakeCaseString;
        }
    }
}
