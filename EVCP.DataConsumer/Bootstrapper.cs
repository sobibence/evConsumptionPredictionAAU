using EasyNetQ;
using Microsoft.Extensions.Configuration;

namespace EVCP.DataConsumer;

public static class Bootstrapper
{
    public static IBus RegisterBus()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        return RabbitHutch.CreateBus(configuration["RabbitMQConnectionString"]);
    }

    public static void RegisterServices()
    {
        // add DI for repositories
    }
}
