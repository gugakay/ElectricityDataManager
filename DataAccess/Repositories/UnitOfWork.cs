using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DefaultDbContext _dbContext;
        private readonly ConcurrentDictionary<Type, Lazy<object>> _repositories = new();

        public UnitOfWork(DefaultDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IRepository<T> GetRepository<T>() where T : class =>
            _repositories
                .GetOrAdd(typeof(IRepository<T>),
                _=>new Lazy<object>(() => new Repository<T>(_dbContext),
                    LazyThreadSafetyMode.None))
                .Value as IRepository<T>;

        public async Task<int> SaveChangesAsync() =>
            await _dbContext.SaveChangesAsync();
    }
}

