namespace EVCP.DataAccess.Repositories;

public interface IBaseRepository<T>
{
    public Task<List<T>> GetAsync();

    public Task<T> GetAsync(int id);
}
