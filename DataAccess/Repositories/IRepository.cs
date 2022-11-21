namespace DataAccess
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAllAsNoTrackingQueryable();
        IQueryable<T> GetAllQueryable();
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        void Delete(T entity);
        void AddRange(List<T> entities);
    }
}
