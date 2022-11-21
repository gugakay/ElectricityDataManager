using DataAccess;
using DataAccess.Entities;

namespace ElectricityDataManager.Services
{
    public interface IElectricityService
    {
        public Task RetrieveDataFromESO(UnitOfWork uow);
        public List<ESOEntity> GetAggregatedData();
    }
}
