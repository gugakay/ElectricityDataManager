using DataAccess.Entities;
using DataAccess;

namespace ElectricityDataManager.Services
{
    public interface ITaskService
    {
        public Task<TaskEntity> RegisterTaskAsync(string name);

        public Task<TaskEntity> IsFinishedAsync(string name, UnitOfWork uow);

        public bool IsRunning(string name);
    }
}