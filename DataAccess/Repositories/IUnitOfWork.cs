namespace DataAccess
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : class;

        Task<int> SaveChangesAsync();
    }
}
