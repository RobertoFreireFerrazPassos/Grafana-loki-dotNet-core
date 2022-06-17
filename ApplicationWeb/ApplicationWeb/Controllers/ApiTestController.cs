using Microsoft.AspNetCore.Mvc;
using LogLibrary;

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

            Logger.Information(data,"got result succesfully");

            return Ok(result);
        }

        [HttpGet("Simple/BadResult")]
        public IActionResult BadResult()
        {
            var result = new
            {
                Message = "Error occurr during saving.",
            };

            Logger.Error(result, result.Message);

            return BadRequest(result);
        }

        [HttpGet("Simple/Exception")]
        public IActionResult Exception()
        {
            int a = 10, b = 0;
            int result;

            try
            {
                result = a / b;

                return Ok(result);
            }
            catch (Exception ex)
            {
                var errorMessage = "Exception while computing {A} / {B}";

                var data = new
                {
                    Message = errorMessage,
                };

                Logger.Error(ex, data, errorMessage);

                return BadRequest(errorMessage);
            }            
        }
    }
}