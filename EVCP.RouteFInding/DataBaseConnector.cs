using EVCP.DataAccess;
using EVCP.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EVCP.RouteFInding;

public class DataBaseConnector
{

    private static readonly object Instancelock = new object();

    private static DataBaseConnector instance = new DataBaseConnector();
    public static DataBaseConnector Instance
    {
        get
        {
            lock (Instancelock)
            {
                if (instance == null)
                {
                    instance = new DataBaseConnector();
                }
            }
            return instance;
        }
    }

    private DataBaseConnector()
    {
        IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddJsonFile("appsettings.json"); // Replace with your configuration file name and path
        IConfiguration configuration = configurationBuilder.Build();

        var serviceProvider = new ServiceCollection()
            .AddSingleton<IConfiguration>(configuration)
            .AddTransient<DapperContext>() // Register the DapperContext
            .AddTransient<IEdgeRepository, EdgeRepository>()
            .AddTransient<IFEstConsumptionRepository, FEstConsumptionRepository>()
            .AddTransient<IFRecordedTravelRepository, FRecordedTravelRepository>()
            .AddTransient<INodeRepository, NodeRepository>()
            .AddTransient<IProducerRepository, ProducerRepository>()
            .AddTransient<IVehicleModelRepository, VehicleModelRepository>()
            .AddTransient<IVehicleTripStatusRepository, VehicleTripStatusRepository>()
            .AddTransient<IWeatherRepository, WeatherRepository>()
            .BuildServiceProvider();

        var dapperContext = serviceProvider.GetService<DapperContext>();
        dapperContext.CreateConnection();
        // Example usage:

    }
}
