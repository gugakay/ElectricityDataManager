using Microsoft.AspNetCore.Http;using System.Globalization;
using CsvHelper;
using DataAccess.Entities;
using System.Net;
using System.Runtime.InteropServices;
using CsvHelper.Configuration;
using System.Linq;
using Microsoft.Extensions.Hosting;
using System.Reflection.PortableExecutable;

namespace ElectricityDataManager.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public FileService(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public void DownloadPublicDataSet(string url)
        {
            string downloadPath = Path.Combine(_hostEnvironment.ContentRootPath, "Files", url.Substring(url.LastIndexOf('/') + 1));

            using (WebClient wc = new WebClient())
            {
                wc.DownloadFile(url, downloadPath);
            }
        }

        public List<T> GetRecordsFromPublicCsvDataSet<T,X>(Func<T, bool>? filter = null) where X : ClassMap
        {
            var filesDirectory = new DirectoryInfo(Path.Combine(_hostEnvironment.ContentRootPath, "Files"));
            var files = filesDirectory.EnumerateFiles();
            var resultList = new List<T>();

            foreach (var file in files)
            {
                using (var reader = File.OpenText(file.FullName))
                {
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        csv.Context.RegisterClassMap<X>();
                        var records = csv.GetRecords<T>();
                        if (filter != null)
                        {
                            records = records.Where(filter);
                        }

                        resultList.AddRange(records);
                    }
                }
            }

            return resultList;
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
