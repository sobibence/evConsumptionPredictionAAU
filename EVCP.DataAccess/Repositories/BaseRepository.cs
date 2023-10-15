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

    public async Task<bool> Create(T entity)
    {
        // sql query
        string columns = string.Join(", ", GetPropertyNames(entity));
        string values = string.Join(",", GetPropertyParameters(entity));
        var query = $"INSERT INTO {_table} ({columns}) VALUES ({values})";

        // create and open database connection
        using var connection = _context.CreateConnection();
        connection.Open();

        // execute query
        var result = await connection.ExecuteAsync(query, entity);

        // return number of rows affected
        return result > 0;
    }

    public virtual async Task<List<T>> GetAsync()
    {
        var query = $"SELECT * FROM {_table}";

        using var connection = _context.CreateConnection();
        connection.Open();

        List<T> result = (await connection.QueryAsync<T>(query)).ToList();

        return result;
    }

    public virtual async Task<T?> GetAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id);

        var query = $"SELECT * FROM {_table}" +
                    $"WHERE id=@Id";

        using var connection = _context.CreateConnection();
        connection.Open();

        T? result = await connection.QueryFirstOrDefaultAsync<T>(query, parameters);

        return result;
    }

    private string[] GetPropertyNames(object entity)
    {
        return entity.GetType()
            .GetProperties()
            .Select(property => property.Name)
            .Where(property => property != "id")
            .ToArray();
    }

    private string[] GetPropertyParameters(object entity)
    {
        return entity.GetType()
            .GetProperties()
            .Select(property => "@" + property.Name)
            .Where(property => property != "@id")
            .ToArray();
    }
}
