using DataAccess;
using DataAccess.Entities;
using ElectricityDataManager.Services;
using ElectricityDataManager.Tests.Common;
using FakeItEasy;
using FluentAssertions;

namespace ElectricityDataManager.Tests.Services
{
    public class ElectricityServiceTests
    {
        private readonly IElectricityService _electricityService;

        public ElectricityServiceTests()
        {
            _electricityService = A.Fake<IElectricityService>();
        }

        [Fact]
        public void ElectricityService_GetAggregatedData()
        {
            //Arrange
            A.CallTo(() => _electricityService.GetAggregatedData()).Returns(TestData.GetESOEntities());

            //Act
            var result = _electricityService.GetAggregatedData();

            //Assert
            result.Should().BeOfType(typeof(List<ESOEntity>));
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
        }
    }
}
