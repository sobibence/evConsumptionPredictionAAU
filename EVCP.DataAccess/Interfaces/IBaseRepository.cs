namespace EVCP.DataAccess.Interfaces;

public interface IBaseRepository<T>
{
    public Task<List<T>> GetAsync();

    public Task<T> GetAsync(int id);
}
