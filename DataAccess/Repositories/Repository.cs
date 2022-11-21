using Microsoft.EntityFrameworkCore;


namespace DataAccess
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;
        private readonly DefaultDbContext _dbContext;


        public Repository(DefaultDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public IQueryable<T> GetAllAsNoTrackingQueryable() =>
            _dbSet.AsNoTracking();

        public IQueryable<T> GetAllQueryable() =>
            _dbSet.AsQueryable();

        public async Task<T> AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _dbSet.AddAsync(entity);
            return entity;
        }

        public void AddRange(List<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var entry = _dbSet.Update(entity);
            entry.OriginalValues.SetValues(await entry.GetDatabaseValuesAsync());

            return entity;
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbSet.Remove(entity);
        }
    }
}
