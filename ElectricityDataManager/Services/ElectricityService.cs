using DataAccess.Entities;
using DataAccess;
using ElectricityDataManager.Infrastructure.Common;
using System.Net;

namespace ElectricityDataManager.Services
{
    public class ElectricityService : IElectricityService
    {
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ElectricityService> _logger;
        private readonly ITaskService _taskService;

        public ElectricityService(IFileService fileService, IUnitOfWork unitOfWork, ILogger<ElectricityService> logger, ITaskService taskService)
        {
            _fileService = fileService;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _taskService = taskService;
        }

        public async Task RetrieveDataFromESO(UnitOfWork uow)
        {
            //i hardcoded this because online dataset is old, this could be automatized
            string firstMonthUrl = @"https://data.gov.lt/dataset/1975/download/10766/2022-04.csv";
            string secondMonthUrl = @"https://data.gov.lt/dataset/1975/download/10766/2022-05.csv";

            var task1 = Task.Run(() => _fileService.DownloadPublicDataSet(firstMonthUrl));
            var task2 = Task.Run(() => _fileService.DownloadPublicDataSet(secondMonthUrl));

            Task.WaitAll(task1, task2);

            try
            {
                var records =
                    _fileService.GetRecordsFromPublicCsvDataSet<ESOEntity, ESOEntityMap>(x => x.Obt_name == "Butas");

                if (records.Count == 0)
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound,
                        "There are no records available");

                var result = records
                    .GroupBy(l => l.Network)
                    .Select(cl => new ESOEntity()
                    {
                        Network = cl.First().Network,
                        Obt_name = cl.First().Obt_name,
                        PL_T = DateTime.Now,
                        P_plus = cl.Sum(c => c.P_plus),
                        P_minus = cl.Sum(c => c.P_minus),
                    }).ToList();

                uow.GetRepository<ESOEntity>().AddRange(result);
                await uow.SaveChangesAsync();

                _fileService.DeleteDownloadedFiles();
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }
            await _taskService.IsFinishedAsync("RetrieveDataFromESO", uow);
        }

        public List<ESOEntity> GetAggregatedData()
        {
            List<ESOEntity> aggregatedData = _unitOfWork.GetRepository<ESOEntity>().GetAllQueryable().ToList();

            return aggregatedData;
        }
    }
}
