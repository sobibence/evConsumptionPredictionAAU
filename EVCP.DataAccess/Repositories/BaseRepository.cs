namespace EVCP.DataAccess.Repositories;

public class BaseRepository<T> : IBaseRepository<T>
{
    private DapperContext _context;

    public BaseRepository(DapperContext context)
    {
        _context = context;
    }

    public virtual async Task<List<T>> GetAsync()
    {
        // query
        // parameters, dymanic parameters

        // open connection
        // get result, query or execute
        // return result

        throw new NotImplementedException();
    }

    public virtual async Task<T> GetAsync(int id)
    {
        throw new NotImplementedException();
    }
}
