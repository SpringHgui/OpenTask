namespace Qz.WebApi.Filters
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Microsoft.OpenApi.Models;
    using System.Text.Json;

    public class LongToStringSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var properties = context.Type.GetProperties()
                .Where(p => p.PropertyType == typeof(long) || p.PropertyType == typeof(long?));

            foreach (var property in properties)
            {
                var propertySchema = schema.Properties
                    .FirstOrDefault(p => string.Equals(p.Key, property.Name, StringComparison.OrdinalIgnoreCase)).Value;
                if (propertySchema != null)
                {
                    propertySchema.Type = "string";
                    propertySchema.Format = "int64";
                }
            }
        }
    }

}
