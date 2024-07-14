// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Dommel;
using System.Text.Json;

namespace OpenTask.Persistence.Base
{
    public class CustomTableNameResolver : ITableNameResolver
    {
        public string ResolveTableName(Type type)
        {
            string camelCaseString = type.Name;

            JsonNamingPolicy namingPolicy = JsonNamingPolicy.SnakeCaseLower;
            string snakeCaseString = namingPolicy.ConvertName(camelCaseString);

            return snakeCaseString; // $"{snakeCaseString.Replace("_entity", string.Empty)}";
        }
    }
}
