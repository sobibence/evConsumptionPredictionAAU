using EasyNetQ;
using EasyNetQ.Management.Client;
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

    public static ManagementClient RegisterManagementClient()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        return new ManagementClient(new Uri("http://localhost:15672"), "guest", "guest");
    }

    public static void RegisterServices()
    {
        // add DI for repositories
    }
}
