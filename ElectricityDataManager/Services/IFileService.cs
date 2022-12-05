using CsvHelper.Configuration;

namespace ElectricityDataManager.Services
{
    public interface IFileService
    {
        public List<T> GetRecordsFromPublicCsvDataSet<T, X>(Func<T, bool> filter) where X : ClassMap;
        public Task<bool> DownloadPublicDataSetAsync(string url);
        public void DeleteDownloadedFiles();
    }
}
