// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using EVCP.DataAccess;
using EVCP.Domain;
using EVCP.Domain.Models;
using EVCP.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

class Program
{
    private readonly IVehicleTripStatusRepository _vehicleTripStatusRepo;
    private readonly ILogger<Program> _logger;

    public Program(ILogger<Program> logger, IVehicleTripStatusRepository vehicleTripStatusRepository)
    {
        _vehicleTripStatusRepo = vehicleTripStatusRepository;
        _logger = logger;
    }

    IHost Host { get; set; }
    public static async Task Main(string[] args)
    {
        HostApplicationBuilder builder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder(args);
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();


        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        builder.Services.AddSingleton<DapperContext>();
        builder.Services.AddTransient<IEdgeRepository, EdgeRepository>();
        builder.Services.AddTransient<IFEstConsumptionRepository, FEstConsumptionRepository>();
        builder.Services.AddTransient<IFRecordedTravelRepository, FRecordedTravelRepository>();
        builder.Services.AddTransient<INodeRepository, NodeRepository>();
        builder.Services.AddTransient<IProducerRepository, ProducerRepository>();
        builder.Services.AddTransient<IVehicleModelRepository, VehicleModelRepository>();
        builder.Services.AddTransient<IVehicleTripStatusRepository, VehicleTripStatusRepository>();
        builder.Services.AddTransient<IWeatherRepository, WeatherRepository>();
        builder.Services.AddSingleton<Program>();
        IHost host = builder.Build();

        Program program = host.Services.GetService<Program>();

        if (program is null)
        {
            Console.WriteLine("program is null");
        }
        else
        {
            program.Host = host;
            await program.Start();
        }

        host.Run();

    }
    async Task Start()
    {
        _logger.LogInformation("Starting program");
        Node node = new Node();
        VehicleTripStatus trip = new VehicleTripStatus
        {
            AdditionalWeightKg = 0,
            VehicleMilageMeters = 0,
            VehicleId = 1
        };
        List<VehicleTripStatus> list = new()
        {
            trip
        };

        var repo = _vehicleTripStatusRepo;
        if (repo is null)
        {
            _logger.LogInformation("repo is null");
        }
        else
        {
            await repo.Create(list);
            var asd = await repo.GetByIdAsync(0);
        }

    }

}