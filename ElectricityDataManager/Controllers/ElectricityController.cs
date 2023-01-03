using ElectricityDataManager.Services;
using Microsoft.AspNetCore.Mvc;
using ElectricityDataManager.Infrastructure.BackgroundWorker;
using IDatabase = StackExchange.Redis.IDatabase;

namespace ElectricityDataManager.Controllers
{
    [Route("electricity")] //    [Route("Electricity")]
    public class ElectricityController : Controller
    {
        private readonly IElectricityService _electricityService;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        private readonly Serilog.ILogger _logger;
        private readonly IDatabase _redisDatabase;

        public ElectricityController(
            IElectricityService electricityService,
            IBackgroundTaskQueue backgroundWorkerQueue,
            Serilog.ILogger logger,
            IDatabase redisDatabase)
        {
            _electricityService = electricityService;
            _backgroundTaskQueue = backgroundWorkerQueue;
            _logger = logger;
            _redisDatabase = redisDatabase;
        }

        [HttpPost("retrieve")] //ElectricityDataEntities
        public IActionResult RetrieveDataFromESO(CancellationToken cancellationToken)
        {
            if(cancellationToken.IsCancellationRequested)
                return BadRequest("Call has been canceled");

            _backgroundTaskQueue.QueueBackgroundWorkItem(async token =>
            {
                await _electricityService.RetrieveDataFromESOAsync();

                _logger.Information($"Done at {DateTime.UtcNow.TimeOfDay}");
            });

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
