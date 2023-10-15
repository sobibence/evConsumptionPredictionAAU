using Dapper;

namespace EVCP.DataAccess.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    private readonly DapperContext _context;
    private readonly string _table;

    public BaseRepository(DapperContext context)
    {
        _context = context;
        _table = typeof(T).Name.ToLower();
    }

    public virtual async Task<List<T>> GetAsync()
    {
        // sql query
        var query = $"SELECT * FROM {_table}";

        // create and open connection to database
        using var connection = _context.CreateConnection();
        connection.Open();

        // execute query and store result
        List<T> result = (await connection.QueryAsync<T>(query)).ToList();

        return result;
    }

    public virtual async Task<T?> GetAsync(int id)
    {
        // parameters for query
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id);

        // sql query
        var query = $"SELECT * FROM {_table}" +
                    $"WHERE id=@Id";

        // create and open connection to database
        using var connection = _context.CreateConnection();
        connection.Open();

        // execute query and store result
        T? result = await connection.QueryFirstOrDefaultAsync<T>(query, parameters);

        return result;
    }
}
