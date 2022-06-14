using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ApplicationWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiTestController : ControllerBase
    {
        private readonly ILogger<ApiTestController> _logger;

        public ApiTestController(ILogger<ApiTestController> logger)
        {
            _logger = logger;
        }

        [HttpGet("Simple/GetResult")]
        public IActionResult GetResult()
        {
            var result = new
            {
                Data = new string[] { "Data 1", "Data 2" },
                Identifier = Guid.NewGuid()
            };

            _logger.LogInformation(message : "GetResult succesfully");

            Log.Information("Serilog Info", result);

            return Ok(result);
        }

        [HttpGet("Simple/BadResult")]
        public IActionResult BadResult()
        {
            var result = new
            {
                message = "Error occurr during saving."
            };

            _logger.LogError(message: result.message);

            Log.Information("Serilog Error", result);

            return BadRequest(result);
        }

        [HttpGet("Simple/Exception")]
        public IActionResult Exception()
        {
            int a = 10, b = 0;
            int result;

            try
            {
                Log.Debug("Result = {A} / {B}", a, b);
                result = a / b;

                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception while computing {A} / {B}");

                return BadRequest("Exception while computing {A} / {B}");
            }            
        }
    }
}