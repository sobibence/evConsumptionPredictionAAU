using Microsoft.Extensions.Configuration;
using Npgsql;

namespace EVCP.DataAccess;

public class DapperContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString = "";

    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetSection("ConnectionStrings")[ConnectionStrings.EVDataWarehouse] ?? "Server=127.0.0.1;Port=5432;Database=EVDataWarehouse;User Id=postgres;Password=server5720;";
    }

    public NpgsqlConnection CreateConnection()
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(_connectionString);
        dataSourceBuilder.UseNetTopologySuite();
        return dataSourceBuilder.Build().CreateConnection();
    }
}
