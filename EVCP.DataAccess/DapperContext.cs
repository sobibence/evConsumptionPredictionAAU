using Microsoft.Extensions.Configuration;
using Npgsql;
using NetTopologySuite.Geometries;

namespace EVCP.DataAccess;

public class DapperContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString = "";

    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetSection("ConnectionStrings")[ConnectionStrings.EVDataWarehouse] ?? "";
    }

    public NpgsqlConnection CreateConnection(){
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(_connectionString);
        dataSourceBuilder.UseNetTopologySuite();
        return dataSourceBuilder.Build().CreateConnection();
    }
}
