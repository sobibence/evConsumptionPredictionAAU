namespace EVCP.DataAccess.Repositories;

public interface IBaseRepository<T>
{
    public Task<bool> Create(T entity);

    public Task<List<T>> GetAsync();

    public Task<T?> GetAsync(int id);
}
