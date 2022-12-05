using DataAccess.Entities;

namespace ElectricityDataManager.Services
{
    public interface IElectricityService
    {
        public Task RetrieveDataFromESOAsync();
        public List<ESOEntity> GetAggregatedData();
    }
}
