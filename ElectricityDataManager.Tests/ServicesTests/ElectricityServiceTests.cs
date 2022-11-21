using Castle.Core.Logging;
using DataAccess;
using DataAccess.Entities;
using ElectricityDataManager.Services;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ElectricityDataManager.Tests.Services
{
    public class ElectricityServiceTests
    {
        private readonly IFileService _fileService;
        private readonly ILogger<ElectricityService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskService _taskService;

        public ElectricityServiceTests()
        {
            _fileService = A.Fake<IFileService>();
            _logger = A.Fake<ILogger<ElectricityService>>();
            _unitOfWork = A.Fake<IUnitOfWork>();
            _taskService = A.Fake<ITaskService>();
        }

        [Fact]
        public void ElectricityService_GetAggregatedData()
        {
            //Arrange
            var records = A.Fake<ICollection<ESOEntity>>();
            var recordList = A.Fake<List<ESOEntity>>();

            var service = new ElectricityService(_fileService, _unitOfWork, _logger, _taskService);

            //Act
            var result = service.GetAggregatedData();

            //Assert
            result.Should().BeOfType(typeof(List<ESOEntity>));
        }
    }
}
