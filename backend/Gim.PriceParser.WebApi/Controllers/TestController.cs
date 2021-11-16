using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gim.PriceParser.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Test()
        {
            foreach (var file in Request.Form.Files)
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    var base64 = Convert.ToBase64String(stream.ToArray());
                    //var result = Xlsx.Parse(base64);
                }

            return Ok();
        }
    }
}