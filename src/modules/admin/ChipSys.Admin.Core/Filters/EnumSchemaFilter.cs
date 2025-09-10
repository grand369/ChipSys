using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using ChipSys.Common.Extensions;

namespace ChipSys.Admin.Core.Filters;

/// <summary>
/// Ã¶¾Ù¼Ü¹¹¹ýÂËÆ÷
/// </summary>
public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var type = context.Type;
        if (type.IsEnum)
        {
            var enumValueType = type.GetField("value__").FieldType;
            var items = Enum.GetValues(type).Cast<Enum>()
            .Where(m => !m.ToString().Equals("Null")).Select(x =>
            $"{x.ToNameWithDescription()}={Convert.ChangeType(x, enumValueType)}").ToList();

            if (items?.Count > 0)
            {
                string description = string.Join(",", items);
                //schema.Extensions.Add("extensions", new OpenApiObject
                //{
                //    ["description"] = new OpenApiString(description)
                //});
                //CommonUtils.GetProperyCommentBySummary
                schema.Description = string.IsNullOrEmpty(schema.Description) ? description : $"{schema.Description}:{description}";
            }
        }
    }
}
