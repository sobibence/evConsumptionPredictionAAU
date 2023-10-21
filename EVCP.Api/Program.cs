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

    // dapper config
    Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
    builder.Services.AddSingleton<DapperContext>();

    // Add services to the container.
    builder.Services.AddTransient<IEdgeRepository, EdgeRepository>();
    builder.Services.AddTransient<INodeRepository, NodeRepository>();
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