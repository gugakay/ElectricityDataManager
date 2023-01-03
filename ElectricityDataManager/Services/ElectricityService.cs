using DataAccess.Entities;
using DataAccess;
namespace ElectricityDataManager.Services
{
    public class ElectricityService : IElectricityService
    {
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Serilog.ILogger _logger;
        private readonly IServiceScopeFactory _serviceProvider;

        private static readonly string s_firstMonthUrl = @"https://data.gov.lt/dataset/1975/download/10766/2022-04.csv";
        private static readonly string s_secondMonthUrl = @"https://data.gov.lt/dataset/1975/download/10766/2022-05.csv";

        public ElectricityService(IFileService fileService, IUnitOfWork unitOfWork, Serilog.ILogger logger, IServiceScopeFactory serviceProvider)
        {
            _fileService = fileService;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task RetrieveDataFromESOAsync()
        {
            _logger.Information($"Retrieving information started at {DateTime.UtcNow.TimeOfDay}");

            try
            {
                if (!await _fileService.DownloadPublicDataSetAsync(s_firstMonthUrl))
                {
                    _logger.Warning("Downloaded file doesn't exist.");
                    return;
                }

                _logger.Information($"File 1 downloaded at {DateTime.UtcNow.TimeOfDay}");

                if (!await _fileService.DownloadPublicDataSetAsync(s_secondMonthUrl))
                {
                    _logger.Warning("Downloaded file doesn't exist.");
                    return;
                }

                _logger.Information($"File 2 downloaded at {DateTime.UtcNow.TimeOfDay}");

                var records =
                    _fileService.GetRecordsFromPublicCsvDataSet<ESOEntity, ESOEntityMap>(x => x.ObtName == "Butas");

                if (records.Count == 0)
                {
                    _logger.Warning("There were no records in downloaded public dataset.");
                    return;
                }

                var result = records
                    .GroupBy(l => l.Network)
                    .Select(cl => new ESOEntity()
                    {
                        Network = cl.First().Network,
                        ObtName = cl.First().ObtName,
                        PlT = DateTime.Now,
                        PPlus = cl.Sum(c => c.PPlus),
                        PMinus = cl.Sum(c => c.PMinus),
                    }).ToList();

                using var scope = _serviceProvider.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                unitOfWork.GetRepository<ESOEntity>().AddRange(result);

                await unitOfWork.SaveChangesAsync();
                _logger.Information($"Done at {DateTime.UtcNow.TimeOfDay}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw;
            }
            finally
            {
                _fileService.DeleteDownloadedFiles();
            }
        }

        public IEnumerable<ESOEntity> GetAggregatedData()
        {
            var aggregatedData = _unitOfWork.GetRepository<ESOEntity>().GetAllQueryable();

            return aggregatedData;
        }
    }
}
