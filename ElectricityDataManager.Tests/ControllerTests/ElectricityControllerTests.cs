using ElectricityDataManager.Services;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using ElectricityDataManager.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Microsoft.Extensions.Logging;

namespace ElectricityDataManager.Tests.Controllers
{
    public  class ElectricityControllerTests
    {
        private readonly IElectricityService _electricityService;
        private readonly ITaskService _taskService;
        private readonly IServiceProvider _serviceProvider;

        public ElectricityControllerTests()
        {
            _electricityService = A.Fake<IElectricityService>();
            _taskService = A.Fake<ITaskService>();
            _serviceProvider = A.Fake<IServiceProvider>();
        }

        [Fact]
        public void ElectricityController_GetAggregatedData()
        {
            //Arrange
            var records = A.Fake<ICollection<ESOEntity>>();
            var recordList = A.Fake<List<ESOEntity>>();

            var controller = new ElectricityController(_electricityService, _taskService, _serviceProvider);

            //Act
            var result = controller.GetAggregatedData();

            //Assert
            result.Should().NotBeNull();
        }
    }
}
