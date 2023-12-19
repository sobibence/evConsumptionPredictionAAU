namespace EVCP.DataAccess.Repositories;

public interface IBaseRepository<T>
{

    public Task<bool> Create(List<T> entities);

    public Task<int?> Create(T entity);

    public Task<IEnumerable<T>> GetAsync();

    public Task<IEnumerable<T>> GetByAsync<S>(string propertyName, S value);

    public Task<T?> GetByIdAsync(int id);

}
