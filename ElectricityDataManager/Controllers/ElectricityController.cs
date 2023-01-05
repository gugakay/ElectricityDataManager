using ElectricityDataManager.Services;
using Microsoft.AspNetCore.Mvc;
using IDatabase = StackExchange.Redis.IDatabase;
using Hangfire;

namespace ElectricityDataManager.Controllers
{
    [Route("electricity")] //    [Route("Electricity")]
    public class ElectricityController : Controller
    {
        private readonly IElectricityService _electricityService;
        private readonly IDatabase _redisDatabase;

        public ElectricityController(
            IElectricityService electricityService,
            IDatabase redisDatabase)
        {
            _electricityService = electricityService;
            _redisDatabase = redisDatabase;
        }

        [HttpPost("retrieve")] //ElectricityDataEntities
        public IActionResult RetrieveDataFromESO(CancellationToken cancellationToken)
        {
            if(cancellationToken.IsCancellationRequested)
                return BadRequest("Call has been canceled");

            BackgroundJob.Enqueue(() => _electricityService.RetrieveDataFromESOAsync());

            return Accepted("Started processing.");
        }


        [HttpGet("retrieve")] //ElectricityDataEntities
        public IActionResult GetAggregatedData()
        {
            var result = _electricityService.GetAggregatedData();

            //var redisResult = _redisDatabase.StringGet("mykey");

            if (result == null)
                return NoContent();

            return Ok(result);
        }
    }
}
