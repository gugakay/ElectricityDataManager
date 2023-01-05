using System.Linq.Expressions;

namespace DataAccess
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAllAsNoTrackingQueryable();
        IQueryable<T> GetAllQueryable();
        IEnumerable<T> Get(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string? includeProperties = "");
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        void Delete(T entity);
        void AddRange(List<T> entities);
    }
}
