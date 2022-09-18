namespace MyAPI.Interfaces
{
    public interface IRepository<T>
    {
        Task<IQueryable<T>> Get(int page, int maxResultsPerPage);

        Task<T?> GetByKey(int key);

        Task<T> Insert(T entity);

        Task<T> Update(int key, T entity);

        Task<int> Delete(int key);
    }
}