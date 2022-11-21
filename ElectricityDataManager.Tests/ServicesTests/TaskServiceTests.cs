using DataAccess;
using DataAccess.Entities;
using ElectricityDataManager.Services;
using FakeItEasy;
using FluentAssertions;
using System.Xml.Linq;

namespace ElectricityDataManager.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly ITaskService _taskService;

        public TaskServiceTests()
        {
            _taskService = A.Fake<ITaskService>();
        }

        [Fact]
        public async void TaskService_RegisterTaskAsync()
        {
            //Arrange
            var taskname = "testtask";

            //Act
            var result = await _taskService.RegisterTaskAsync(taskname);

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void TaskService_IsRunning()
        {
            //Arrange
            var taskname = "testtask";

            //Act
            var result = _taskService.IsRunning(taskname);

            //Assert
            result.Should().BeFalse();
        }
    }
}
