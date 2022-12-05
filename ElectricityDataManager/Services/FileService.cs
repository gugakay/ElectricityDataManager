using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace ElectricityDataManager.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Serilog.ILogger _logger;

        public FileService(IWebHostEnvironment hostEnvironment, IHttpClientFactory httpClientFactory, Serilog.ILogger logger)
        {
            _hostEnvironment = hostEnvironment;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<bool> DownloadPublicDataSetAsync(string url)
        {
            string downloadPath = Path.Combine(_hostEnvironment.ContentRootPath, "Files", url[(url.LastIndexOf('/') + 1)..]);

            var httpClient = _httpClientFactory.CreateClient();
            var stream = await httpClient.GetStreamAsync(url);

            using (var fileStream = File.Create(downloadPath))
            {
                stream.CopyTo(fileStream);
            }

            return File.Exists(downloadPath);
        }

        public List<T> GetRecordsFromPublicCsvDataSet<T,X>(Func<T, bool>? filter = null) where X : ClassMap
        {
            try
            {
                var csvFilesDirectory = new DirectoryInfo(Path.Combine(_hostEnvironment.ContentRootPath, "Files"));
                var csvFiles = csvFilesDirectory.EnumerateFiles();
                var resultList = new List<T>();

                foreach (var file in csvFiles)
                {
                    using var reader = File.OpenText(file.FullName);
                    using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                    csv.Context.RegisterClassMap<X>();
                    var records = csv.GetRecords<T>();
                    if (filter != null)
                    {
                        records = records.Where(filter);
                    }

                    resultList.AddRange(records);
                }

                return resultList;
            }

            catch (DirectoryNotFoundException ex)
            {
                _logger.Error(ex, "Directory not found while getting records from public csv data set.");
                throw;
            }
        }

        public void DeleteDownloadedFiles()
        {
            string[] files = Directory.GetFiles(Path.Combine(_hostEnvironment.ContentRootPath, "Files"));
            foreach (string file in files)
            {
                File.Delete(file);
            }
        }
    }
}
