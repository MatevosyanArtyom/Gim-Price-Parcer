using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Gim.PriceParser.WebApi.Util
{
    public class MakeOperationIdFilter: IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor is ControllerActionDescriptor desc)
            {
                operation.OperationId = desc.ControllerName + desc.ActionName;
            }
        }
    }
}
