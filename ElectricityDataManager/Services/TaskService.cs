using Microsoft.AspNetCore.Http;using System.Globalization;
using CsvHelper;
using DataAccess.Entities;
using System.Net;
using System.Runtime.InteropServices;
using CsvHelper.Configuration;
using System.Linq;
using Microsoft.Extensions.Hosting;
using System.Reflection.PortableExecutable;
using DataAccess;

namespace ElectricityDataManager.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TaskEntity> RegisterTaskAsync(string name)
        {
            var task = new TaskEntity
            {
                TaskName = name,
                CreationTime = DateTime.Now,
                IsRunning = true
            };

            var entity = await _unitOfWork.GetRepository<TaskEntity>().AddAsync(task);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<TaskEntity> IsFinishedAsync(string name, UnitOfWork uow)
        {
            var taskEntity = uow.GetRepository<TaskEntity>().GetAllQueryable()
                .Where(x => x.TaskName == name && x.IsRunning).SingleOrDefault();

            TaskEntity entity = new TaskEntity();

            if (taskEntity != null)
            {
                taskEntity.EndTime = DateTime.Now;
                taskEntity.IsRunning = false;
                entity = await uow.GetRepository<TaskEntity>().UpdateAsync(taskEntity);
                await uow.SaveChangesAsync();
            }
            return entity;
        }

        public bool IsRunning(string name)
        {
            return _unitOfWork.GetRepository<TaskEntity>().GetAllQueryable()
                .Where(x => x.TaskName == name && x.IsRunning).Any();
        }
    }
}
