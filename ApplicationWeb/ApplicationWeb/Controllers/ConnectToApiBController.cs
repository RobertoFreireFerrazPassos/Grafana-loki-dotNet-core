using Microsoft.AspNetCore.Mvc;
using LogLibrary;

namespace ApplicationWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectToApiBController : ControllerBase
    {
        [HttpGet("GetValue")]
        public IActionResult GetResult()
        {
            Random r = new Random();

            int result = r.Next(0, 100);

            var data = new
            {
                Result = result,
            };

            Logger.Information(data);

            return Ok();
        }
    }
}
