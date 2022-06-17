using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ApplicationWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiTestController : ControllerBase
    {
        [HttpGet("Simple/GetResult")]
        public IActionResult GetResult()
        {
            var result = new
            {
                Data = new string[] { "Data 1", "Data 2" },
                Identifier = Guid.NewGuid()
            };

            var data = new
            {
                Result = result,
            };

            Log.ForContext("Extra", data)
               .Information("GetResult succesfully");

            return Ok(result);
        }

        [HttpGet("Simple/BadResult")]
        public IActionResult BadResult()
        {
            var result = new
            {
                Message = "Error occurr during saving."
            };

            var data = new
            {
                Message = result.Message,
            };

            Log.ForContext("Error", data)
               .Information("Serilog Error" + result);

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
                var data = new
                {
                    Message = "Exception while computing {A} / {B}",
                };

                Log.ForContext("Error", data)
                   .Error(ex, "Exception while computing {A} / {B}");

                return BadRequest("Exception while computing {A} / {B}");
            }            
        }
    }
}