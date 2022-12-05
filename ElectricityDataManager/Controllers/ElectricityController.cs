using ElectricityDataManager.Services;
using Microsoft.AspNetCore.Mvc;
using ElectricityDataManager.Infrastructure.BackgroundWorker;

namespace ElectricityDataManager.Controllers
{
    [Route("Electricity")]
    public class ElectricityController : Controller
    {
        private readonly IElectricityService _electricityService;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        private readonly Serilog.ILogger _logger;

        public ElectricityController(
            IElectricityService electricityService,
            IBackgroundTaskQueue backgroundWorkerQueue,
            Serilog.ILogger logger)
        {
            _electricityService = electricityService;
            _backgroundTaskQueue = backgroundWorkerQueue;
            _logger = logger;
        }

        [HttpPost("ElectricityDataEntities")]
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


        [HttpGet("ElectricityDataEntities")]
        public IActionResult GetAggregatedData()
        {
            var result = _electricityService.GetAggregatedData();

            if (result == null)
                return NoContent();

            return Ok(result);
        }
    }
}
