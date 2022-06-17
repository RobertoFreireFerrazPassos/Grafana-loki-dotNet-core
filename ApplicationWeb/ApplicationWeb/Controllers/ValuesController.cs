using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Text.Json;

namespace ApplicationWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
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

            Log.ForContext("Extra", data)
               .Information(JsonSerializer.Serialize(data));

            return Ok(result);
        }
    }
}
