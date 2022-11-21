using CsvHelper.Configuration;
using DataAccess.Entities;
using System.Linq.Expressions;

namespace ElectricityDataManager.Services
{
    public interface IFileService
    {
        public List<T> GetRecordsFromPublicCsvDataSet<T, X>(Func<T, bool> filter = null) where X : ClassMap;
        public void DownloadPublicDataSet(string url);
        public void DeleteDownloadedFiles();
    }
}
