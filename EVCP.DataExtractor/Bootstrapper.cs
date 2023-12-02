using EasyNetQ;
using Microsoft.Extensions.Configuration;

namespace EVCP.DataPublisher
{
    public static class Bootstrapper
    {
        public static IBus RegisterBus()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            return RabbitHutch.CreateBus(configuration["RabbitMQConnectionString"]);
        }
    }
}
