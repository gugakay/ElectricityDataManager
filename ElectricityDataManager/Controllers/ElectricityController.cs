using ElectricityDataManager.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Net.Mime;
using Serilog;
using DataAccess.Entities;
using ElectricityDataManager.Infrastructure.Common;
using DataAccess;

namespace ElectricityDataManager.Controllers
{
    [Route("Electricity")]
    public class ElectricityController : Controller
    {
        private readonly IElectricityService _electricityService;
        private readonly ITaskService _taskService;
        private readonly IServiceProvider _serviceProvider;

        public ElectricityController(IElectricityService electricityService, ITaskService taskService, IServiceProvider serviceProvider)
        {
            _electricityService = electricityService;
            _taskService = taskService;
            _serviceProvider = serviceProvider;
        }

        [HttpGet("RetrieveDataFromESO")]
        public async Task<CommonResponse> RetrieveDataFromESO()
        {
            if (_taskService.IsRunning("RetrieveDataFromESO"))
                return new CommonResponse() { Message = "Try another time." };
            else
            {
                await _taskService.RegisterTaskAsync("RetrieveDataFromESO");

                _ = Task.Run(async () =>
                {
                    await using (var scope = _serviceProvider.CreateAsyncScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<DefaultDbContext>();

                        await _electricityService.RetrieveDataFromESO(new UnitOfWork(context));
                    }
                });

                return new CommonResponse() { Message = "Started processing." };
            }
        }


        [HttpGet("GetAggregatedData")]
        public CommonResponse<List<ESOEntity>> GetAggregatedData()
        {
            CommonResponse<List<ESOEntity>> response = new CommonResponse<List<ESOEntity>>();

            var result = _electricityService.GetAggregatedData();

            if (result != null)
            {
                response.Data = result;
                response.StatusCode = 1;
                response.Message = "OK";
            }

            return response;
        }
    }
}
