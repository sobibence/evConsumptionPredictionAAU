using EasyNetQ;
using EVCP.DataExtractor;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var bus = RabbitHutch.CreateBus(configuration["RabbitMQConnectionString"]);
var messagePublisher = new EVDataPublisher(bus);

var worker = new Worker(messagePublisher);

worker.Run().Wait();

// test consumer
bus.PubSub.Subscribe<IEVData>("test", HandleMessage);
Console.WriteLine("Listening for messages. Hit <return> to quit.");
Console.ReadLine();

void HandleMessage(IEVData message)
{
    Console.WriteLine(message.Name);
}