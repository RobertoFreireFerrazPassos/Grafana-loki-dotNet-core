using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ApplicationWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;

        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetValue")]
        public IActionResult GetResult()
        {
            Random r = new Random();

            int result = r.Next(0, 100);

            _logger.LogInformation(message: result.ToString());

            Log.Information("Serilog Values", result);

            return Ok(result);
        }
    }
}
