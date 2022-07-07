using Microsoft.AspNetCore.Mvc;
using LogLibrary;
using System;

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

            Logger.Information(dataRequest, "ApiTest_SaveData");

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

            Logger.Information(data, "ApiTest_GetResult");

            return Ok(result);
        }

        [HttpGet("BadResult")]
        public IActionResult BadResult()
        {
            var result = new
            {
                Message = "Error occurr during saving.",
            };

            Logger.Information(result, "ApiTest_BadResult", result.Message);

            return BadRequest(result);
        }

        [HttpGet("Exception")]
        public IActionResult Exception()
        {
            int a = 10, b = 0;

            try
            {
                var result = a / b;

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "ApiTest_Exception", "Exception while computing A/B");

                return BadRequest("Exception while computing A/B");
            }  
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

            Logger.Information(data, "ApiTest_GetValue");

            return Ok(result);
        }
    }
}