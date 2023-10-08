﻿using Microsoft.Extensions.Configuration;
using Npgsql;

namespace EVCP.DataAccess;

public class DbContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString = "";

    public DbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetSection("ConnectionStrings")["EVDataWarehouse"] ?? "";
    }

    public NpgsqlConnection CreateConnection()
        => new NpgsqlConnection(_connectionString);
}
