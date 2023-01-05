using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;


        public Repository(DefaultDbContext dbContext)
        {
            _dbSet = dbContext.Set<T>();
        } 

        public IQueryable<T> GetAllAsNoTrackingQueryable() =>
            _dbSet.AsNoTracking();

        public IQueryable<T> GetAllQueryable() =>
            _dbSet.AsQueryable();

        public virtual IEnumerable<T> Get(Expression<Func<T, bool>>? filter = null,
                                            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                            string? includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if(!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

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
            var propertyValues = await entry.GetDatabaseValuesAsync();

            if (propertyValues == null)
            {
                throw new Exception("Entity not found");
            }
            else
            {
                entry.OriginalValues.SetValues(propertyValues);
            }

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
