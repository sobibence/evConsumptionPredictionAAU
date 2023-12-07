// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using EVCP.DataAccess;
using EVCP.Domain;
using EVCP.Domain.Models;
using EVCP.Domain.Repositories;
using EVCP.RouteFinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;
using EVCP.Controllers.PathController;
using EVCP.MachineLearningModelClient;

class Program
{
    private readonly IDataBaseConnector dataBase;
    private readonly ILogger<Program> _logger;

    public static ServiceProvider? service;

    public Program(ILogger<Program> logger, IDataBaseConnector dataBaseConnector)
    {
        dataBase = dataBaseConnector;
        _logger = logger;
    }

    public static async Task Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<Benchmarker>(ManualConfig.Create(DefaultConfig.Instance).WithOptions(ConfigOptions.DisableOptimizationsValidator));
        //RegisterServices().GetService<IPathController>().GetBestPathAsync(0, 3112, 29653).Wait();
    }

    public static ServiceProvider RegisterServices()
    {
        if (service is not null)
        {
            return service;
        }
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        var serviceProvider = new ServiceCollection()
            .AddLogging(logging =>
                {
                    logging.AddConsole();
                    logging.AddDebug();
                })
            .AddSingleton(provider => new DapperContext(GetConfiguration()))
            .AddTransient<IFEstConsumptionRepository, FEstConsumptionRepository>()
            .AddTransient<IEdgeInfoRepository, EdgeInfoRepository>()
            .AddTransient<INodeRepository, NodeRepository>()
            .AddTransient<IMapConstructionRepository, MapConstructionRepository>()
            .AddTransient<IWeatherRepository, WeatherRepository>()
            .AddTransient<IVehicleTripStatusRepository, VehicleTripStatusRepository>()
            .AddTransient<IMachineLearningModelService, MachineLearningModelService>()
            .AddTransient<IEdgeRepository, EdgeRepository>()
            .AddTransient<IPathController, PathController>()
            .BuildServiceProvider();
        service = serviceProvider;
        return serviceProvider;
    }

    private static IConfiguration GetConfiguration()
    => new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();


}
