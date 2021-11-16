using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Gim.PriceParser.WebApi.Controllers
{
    public abstract class ApiControllerBase : ControllerBase
    {
        /// <summary>
        ///     Содержит идентификатор пользователя из клейма
        /// </summary>
        protected string CurrentUserId
        {
            get
            {
                // Определить Id пользователя из клейма
                var userIdClaim = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Sid);

                return userIdClaim?.Value;
            }
        }
    }
}