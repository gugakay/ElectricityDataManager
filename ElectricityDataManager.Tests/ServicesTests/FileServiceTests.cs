using DataAccess.Entities;
using ElectricityDataManager.Services;
using ElectricityDataManager.Tests.Common;
using FakeItEasy;
using FluentAssertions;

namespace ElectricityDataManager.Tests.ServicesTests
{
    public class FileServiceTests
    {
        private readonly IFileService _fileService;

        public FileServiceTests()
        {
            _fileService = A.Fake<IFileService>();
        }

        [Fact]
        public void FileService_DownloadPublicDataSetAsync()
        {
            //Arrange
            A.CallTo(() => _fileService.DownloadPublicDataSetAsync("test")).Returns(Task.FromResult(true));

            //Act
            var result = _fileService.DownloadPublicDataSetAsync("test");

            //Assert
            A.CallTo(() => _fileService.DownloadPublicDataSetAsync("test")).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void FileService_GetRecordsFromPublicCsvDataSet()
        {
            //Arrange
            Func<ESOEntity, bool>? filter = null;
            A.CallTo(() => _fileService.GetRecordsFromPublicCsvDataSet<ESOEntity, ESOEntityMap>(filter)).Returns(TestData.GetESOEntities());

            //Act
            var result = _fileService.GetRecordsFromPublicCsvDataSet<ESOEntity, ESOEntityMap>(filter);

            //Assert
            result.Should().BeOfType(typeof(List<ESOEntity>));
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
        }

        [Fact]
        public void FileService_DeleteDownloadedFiles()
        {
            //Arrange
            A.CallTo(() => _fileService.DeleteDownloadedFiles());

            //Act
            _fileService.DeleteDownloadedFiles();

            //Assert
            A.CallTo(() => _fileService.DeleteDownloadedFiles()).MustHaveHappenedOnceExactly();
        }
    }
}
