using EasyNetQ;
using EVCP.DataAccess;
using EVCP.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EVCP.DataConsumer;

public static class Bootstrapper
{
    public static IBus RegisterBus()
    {
        var configuration = GetConfiguration();

        return RabbitHutch.CreateBus(configuration["RabbitMQConnectionString"]);
    }

    public static ServiceProvider RegisterServices()
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddSingleton(provider => new DapperContext(GetConfiguration()))
            .AddScoped<IEdgeRepository, EdgeRepository>()
            .AddScoped<IFEstConsumptionRepository, FEstConsumptionRepository>()
            .AddScoped<IFRecordedTravelRepository, FRecordedTravelRepository>()
            .AddScoped<IVehicleModelRepository, VehicleModelRepository>()
            .AddScoped<IWeatherRepository, WeatherRepository>()
            .BuildServiceProvider();

        return serviceProvider;
    }

    private static IConfiguration GetConfiguration()
    => new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
}
