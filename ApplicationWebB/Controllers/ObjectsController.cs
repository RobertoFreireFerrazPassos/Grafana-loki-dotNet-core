using Microsoft.AspNetCore.Mvc;
using LogLibrary;

namespace ApplicationWebB.Controllers
{
    public class Data
    {
        public string Name { get; set; }
        public string[] Values { get; set; }
        public IEnumerable<string> List { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class ObjectsController : ControllerBase
    {
        [HttpPost("SaveData")]
        public IActionResult SaveData(Data dataRequest)
        {
            if (dataRequest?.Values is not null)
            {
                dataRequest.List = dataRequest.Values.ToList();
            }

            Logger.Information(dataRequest, "LongRequest succesfully");

            return Ok(dataRequest);
        }

        [HttpGet("LongRequest")]
        public async Task<IActionResult> LongRequest(CancellationToken ct)
        {
            try
            {
                await Task.Delay(10000, ct);
                Logger.Information(default(object), "LongRequest succesfully");

                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, default(object), "Error during LongRequest");
                return BadRequest();
            } 
        }
    }
} 