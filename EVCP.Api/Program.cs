using EVCP.Controllers.PathController;
using EVCP.DataAccess;
using EVCP.Domain.Repositories;
using EVCP.MachineLearningModelClient;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Logging
    builder.Host.UseSerilog();

    // Dapper config
    Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
    builder.Services.AddSingleton<DapperContext>();

    // Add DI for repositories.
    builder.Services.AddTransient<IEdgeRepository, EdgeRepository>();
    builder.Services.AddTransient<IFEstConsumptionRepository, FEstConsumptionRepository>();
    builder.Services.AddTransient<IFRecordedTravelRepository, FRecordedTravelRepository>();
    builder.Services.AddTransient<INodeRepository, NodeRepository>();
    builder.Services.AddTransient<IProducerRepository, ProducerRepository>();
    builder.Services.AddTransient<IVehicleModelRepository, VehicleModelRepository>();
    builder.Services.AddTransient<IVehicleTripStatusRepository, VehicleTripStatusRepository>();
    builder.Services.AddTransient<IWeatherRepository, WeatherRepository>();
    builder.Services.AddTransient<IMachineLearningModelService, MachineLearningModelService>();
    builder.Services.AddTransient<IPathController, PathController>();

    builder.Services.AddControllers();
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