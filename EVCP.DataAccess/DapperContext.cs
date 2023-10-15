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
        _connectionString = _configuration.GetSection("ConnectionStrings")[ConnectionStrings.EVDataWarehouse] ?? "";
    }

    public NpgsqlConnection CreateConnection()
        => new NpgsqlConnection(_connectionString);
}
