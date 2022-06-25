using Microsoft.AspNetCore.Mvc;
using LogLibrary;
using Serilog;
using System.Text.Json;

namespace ApplicationWeb.Controllers
{
    public class Data
    {
        public string Name { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class ApiTestController : ControllerBase
    {
        [HttpPost("SaveData")]
        public IActionResult SaveData(Data dataRequest)
        {
            var message = "Save Data succesfully";

            Logger.Information(dataRequest);

            return Ok(message);
        }

        [HttpGet("GetResult")]
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

        [HttpGet("BadResult")]
        public IActionResult BadResult()
        {
            var result = new
            {
                Message = "Error occurr during saving.",
            };

            Logger.Error(result, result.Message);

            return BadRequest(result);
        }

        [HttpGet("GetValue")]
        public IActionResult GetValue()
        {
            Random r = new Random();

            int result = r.Next(0, 100);

            var data = new
            {
                Result = result,
            };

            Logger.Information(data);

            return Ok(result);
        }
    }
}