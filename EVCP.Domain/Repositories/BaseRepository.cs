using Dapper;
using EVCP.DataAccess;
using EVCP.DataAccess.Repositories;
using EVCP.Domain.Helpers;
using System.Reflection;

namespace EVCP.Domain.Repositories;

public class BaseRepository<T> : IBaseRepository<T>
{
    private readonly DapperContext _context;

    public readonly string Table;

    public BaseRepository(DapperContext context)
    {
        _context = context;
        Table = GetTableName();
    }

    public virtual async Task<bool> Create(T entity)
    {
        if (entity == null) return false;

        // generate insert sql query
        string columns = string.Join(", ", GetPropertyNames(entity));
        string values = string.Join(",", GetPropertyParameters(entity));
        var query = $"INSERT INTO {Table} ({columns}) VALUES ({values})";

        // create and open database connection
        using var connection = _context.CreateConnection();
        connection.Open();

        // execute query
        var result = await connection.ExecuteAsync(query, entity);

        // return number of rows affected
        return result > 0;
    }

    public virtual async Task<IEnumerable<T>> GetAsync()
    {
        var query = $"SELECT * FROM {Table}";

        using var connection = _context.CreateConnection();
        connection.Open();

        List<T> result = (await connection.QueryAsync<T>(query)).ToList();

        return result;
    }

    public virtual async Task<T?> GetAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id);

        var query = $"SELECT * FROM {Table}" +
                    $"WHERE id=@Id";

        using var connection = _context.CreateConnection();
        connection.Open();

        T? result = await connection.QueryFirstOrDefaultAsync<T>(query, parameters);

        return result;
    }

    private string[] GetPropertyNames(T entity)
    {
        return entity.GetType()
            .GetProperties()
            .Where(property => !Attribute.IsDefined(property, typeof(OnInsertIgnore)))
            .Select(property => property.Name)
            .ToArray();
    }

    private string[] GetPropertyParameters(T entity)
    {
        return entity.GetType()
            .GetProperties()
            .Where(property => !Attribute.IsDefined(property, typeof(OnInsertIgnore)))
            .Select(property =>
            {
                if (Attribute.IsDefined(property, typeof(EnumType)))
                    return $"'@{property.Name}'";
                else
                    return "@" + property.Name;
            })

            .ToArray();
    }

    private string GetTableName()
    {
        Type type = typeof(T);
        var result = type.Name.ToLower();

        var attribute = (TableName)type.GetCustomAttribute(typeof(TableName), false);

        if (attribute != null)
        {
            result = attribute.Name;
        }

        return result;
    }
}
