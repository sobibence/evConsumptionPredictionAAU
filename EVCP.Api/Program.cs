using EVCP.DataAccess;
using EVCP.Domain.Repositories;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    // add enum mapping
    //string connectionString = builder.Configuration.GetSection("ConnectionStrings")[ConnectionStrings.EVDataWarehouse] ?? "";

    //var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
    //dataSourceBuilder.MapEnum<RoadType>("road_type");
    //dataSourceBuilder.MapEnum<WindDirection>("wind_direction");
    //await using var dataSource = dataSourceBuilder.Build();

    // Add services to the container.
    builder.Services.AddSingleton<DapperContext>();

    builder.Services.AddTransient<IWeatherRepository, WeatherRepository>();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}