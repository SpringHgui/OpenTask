// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Swashbuckle.AspNetCore.SwaggerGen;

namespace OpenTask.WebApi
{
    public class MySchemaFilter : ISchemaFilter
    {
        public void Apply(Microsoft.OpenApi.Models.OpenApiSchema schema, SchemaFilterContext context)
        {
            foreach (KeyValuePair<string, Microsoft.OpenApi.Models.OpenApiSchema> item in schema.Properties)
            {
                if (item.Value.Type == "integer" && item.Value.Format == "int64")
                {
                    item.Value.Type = "string";
                }
            }
        }
    }

    public class MyParameterFilter : IParameterFilter
    {
        public void Apply(Microsoft.OpenApi.Models.OpenApiParameter parameter, ParameterFilterContext context)
        {
            if (parameter.Schema.Type == "integer" && parameter.Schema.Format == "int64")
            {
                parameter.Schema.Type = "string";
            }
        }
    }
}
