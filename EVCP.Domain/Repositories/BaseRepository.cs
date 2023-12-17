using Dapper;
using EVCP.DataAccess;
using EVCP.DataAccess.Repositories;
using EVCP.Domain.Helpers;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;

namespace EVCP.Domain.Repositories;

public class BaseRepository<T> : IBaseRepository<T>
{
    private readonly ILogger<BaseRepository<T>> _logger;

    public readonly DapperContext _context;
    public readonly string Table;

    public BaseRepository(ILogger<BaseRepository<T>> logger, DapperContext context)
    {
        _logger = logger;
        _context = context;
        Table = GetTableName();
    }

    public virtual async Task<bool> Create(List<T> entities)
    {
        var result = false;

        using var connection = _context.CreateConnection();
        connection.Open();
        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                foreach (var entity in entities)
                {
                    if (entity == null) return false;

                    (var columnArr, var valueArr, var parameters) = GetForInsert(entity);

                    string columns = string.Join(", ", columnArr);
                    string values = string.Join(",", valueArr);
                    var query = $"INSERT INTO {Table} ({columns}) VALUES ({values})";

                    await connection.ExecuteAsync(query, parameters);
                }

                transaction.Commit();
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex}");
                transaction.Rollback();
            }
        }
        connection.Close();

        connection.Close();

        return result;
    }

    public virtual async Task<bool> Create(T entity)
    {
        if (entity == null) return false;

        // generate insert sql query and dynamic parameters
        (var columnArr, var valueArr, var parameters) = GetForInsert(entity);

        string columns = string.Join(", ", columnArr);
        string values = string.Join(",", valueArr);
        var query = $"INSERT INTO {Table} ({columns}) VALUES ({values})";

        // open database connection
        using var connection = _context.CreateConnection();
        connection.Open();

        // execute query
        var result = await connection.ExecuteAsync(query, parameters);
        connection.Close();
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
        connection.Close();
        return result;
    }

    /// <summary>
    /// Get by property of model method.
    /// </summary>
    /// <typeparam name="S">type of value</typeparam>
    /// <param name="propertyName">model property name</param>
    /// <param name="value">value of property</param>
    /// <returns>List of models filtered by value of property</returns>
    public async Task<IEnumerable<T>> GetByAsync<S>(string propertyName, S value)
    {
        var property = typeof(T).GetProperty(propertyName);

        if (property is null || value is null) return new List<T>();

        var columnArr = GetForSelect();
        var columns = string.Join(", ", columnArr);
        var columnName = GetColumnNameByProperty(property);

        var queryValue = "@Value";
        var parameters = new DynamicParameters();

        if (Attribute.IsDefined(property, typeof(EnumType)))
        {
            queryValue = $"{queryValue}::{columnName}";
            parameters.Add("@Value", value.ToString());
        }
        else
        {
            parameters.Add("@Value", value);
        }

        var query = $"SELECT {columns} FROM {Table} " +
                    $"WHERE {columnName}={queryValue}";

        using var connection = _context.CreateConnection();
        connection.Open();

        var result = await connection.QueryAsync<T>(query, parameters);
        connection.Close();
        return result;
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id);

        var columnArr = GetForSelect();
        var columns = string.Join(", ", columnArr);
        var query = $"SELECT {columns} FROM {Table} " +
                    $"WHERE id=@Id";

        using var connection = _context.CreateConnection();
        connection.Open();

        T? result = await connection.QueryFirstOrDefaultAsync<T>(query, parameters);
        connection.Close();
        return result;
    }

    #region Private methods
    protected (string[] columns, string[] values, DynamicParameters parameters) GetForInsert(T entity)
    {
        var columnNames = new List<string>();
        var propertyNames = new List<string>();
        var parameters = new DynamicParameters();

        entity.GetType()
            .GetProperties()
            .Where(property => !Attribute.IsDefined(property, typeof(OnInsertIgnore)))
            .Where(property => !Attribute.IsDefined(property, typeof(NotMappedAttribute)))
            .ToList()
            .ForEach(property =>
            {
                // add db column name
                var column = GetColumnNameByProperty(property);
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

    protected string[] GetForSelect()
    {
        var columnToProperty = new List<string>();

        typeof(T).GetProperties()
            .Where(property => !Attribute.IsDefined(property, typeof(NotMappedAttribute)))
            .ToList()
            .ForEach(property =>
            {
                var columnName = GetColumnNameByProperty(property);
                var propertyName = property.Name;

                columnToProperty.Add($"{columnName} AS {property.Name}");
            });

        return columnToProperty.ToArray();
    }

    protected string GetTableName()
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

    protected string GetColumnNameByProperty(PropertyInfo property)
    {
        var attribute = (ColumnName)property.GetCustomAttribute(typeof(ColumnName), false);
        var columnName = attribute != null ? attribute.Name : "";

        return columnName;
    }
    #endregion
}
