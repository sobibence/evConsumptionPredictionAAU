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

        // generate insert sql query and dynamic parameters
        (var columnArr, var valueArr, var parameters) = GetForInsert(entity);

        string columns = string.Join(", ", columnArr);
        string values = string.Join(",", valueArr);
        var query = $"INSERT INTO {Table} ({columns}) VALUES ({values})";

        // create and open database connection
        using var connection = _context.CreateConnection();
        connection.Open();

        // execute query
        var result = await connection.ExecuteAsync(query, parameters);

        // return number of rows affected
        return result > 0;
    }

    public virtual async Task<IEnumerable<T>> GetAsync()
    {
        var columnArr = GetForSelect();
        var columns = string.Join(", ", columnArr);
        var query = $"SELECT {columns} FROM {Table}";

        using var connection = _context.CreateConnection();
        connection.Open();

        List<T> result = (await connection.QueryAsync<T>(query)).ToList();

        return result;
    }

    public virtual async Task<T?> GetAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id);

        var columnArr = GetForSelect();
        var columns = string.Join(", ", columnArr);
        var query = $"SELECT {columns} FROM {Table}" +
                    $"WHERE id=@Id";

        using var connection = _context.CreateConnection();
        connection.Open();

        T? result = await connection.QueryFirstOrDefaultAsync<T>(query, parameters);

        return result;
    }

    private (string[] columns, string[] values, DynamicParameters parameters) GetForInsert(T entity)
    {
        var columnNames = new List<string>();
        var propertyNames = new List<string>();
        var parameters = new DynamicParameters();

        entity.GetType()
            .GetProperties()
            .Where(property => !Attribute.IsDefined(property, typeof(OnInsertIgnore)))
            .ToList()
            .ForEach(property =>
            {
                // add db column name
                var attribute = (ColumnName)property.GetCustomAttribute(typeof(ColumnName), false);
                var column = attribute != null ? attribute.Name : "";
                columnNames.Add(column);

                var propertyName = $"@{property.Name}";

                // add model property names and dynamic parameters
                if (Attribute.IsDefined(property, typeof(EnumType)))
                {
                    propertyNames.Add($"{propertyName}::{column}");
                    parameters.Add(propertyName, property.GetValue(entity).ToString());
                }
                else
                {
                    propertyNames.Add(propertyName);
                    parameters.Add(propertyName, property.GetValue(entity));
                }
            });

        return (columnNames.ToArray(), propertyNames.ToArray(), parameters);
    }

    private string[] GetForSelect()
    {
        var columnToProperty = new List<string>();

        typeof(T).GetProperties()
            .ToList()
            .ForEach(property =>
            {
                var attribute = (ColumnName)property.GetCustomAttribute(typeof(ColumnName), false);
                var columnName = attribute != null ? attribute.Name : "";
                var propertyName = property.Name;

                columnToProperty.Add($"{columnName} AS {property.Name}");
            });

        return columnToProperty.ToArray();
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
