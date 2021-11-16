using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Gim.PriceParser.WebApi.Util
{
    public class EnumAsSeparateTypeFilter: ISchemaFilter
    {
        public void Apply(OpenApiSchema model, SchemaFilterContext context)
        {
            var consideredType = context.Type.GetThisOrUnderlyingNullableType();
            if (!consideredType.IsEnum)
            {
                return;
            }

            var enumName = consideredType.Name;

            var @enum = model.Enum;
            model.Enum = null;
            model.Reference = new OpenApiReference {ExternalResource = $"#/definitions/{enumName}", Id = $"/definitions/{enumName}"};
            model.Type = null;

            if (context.SchemaRepository.Schemas.Any(x => x.Key == enumName))
            {
                return;
            }

            context.SchemaRepository.Schemas.Add(enumName, new OpenApiSchema
            {
                Enum = @enum,
                Type = "string"
            });
        }
    }
}
