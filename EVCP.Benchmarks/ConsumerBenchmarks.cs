using BenchmarkDotNet.Attributes;
using EasyNetQ;
using EasyNetQ.Management.Client;
using EVCP.DataConsumer;
using EVCP.DataConsumer.Consumer;
using EVCP.DataConsumer.Publisher;
using EVCP.Domain.Repositories;
using EVCP.Dtos;
using Microsoft.Extensions.DependencyInjection;

namespace EVCP.Benchmarks;

[SimpleJob(iterationCount: 3, warmupCount: 0)]
[MemoryDiagnoser]
[MinColumn, MaxColumn, MeanColumn, MedianColumn]
public class ConsumerBenchmarks
{
    //[Params(10, 100, 1000, 10000)]
    [Params(1)]
    public int NoOfMessages { get; set; }

    //[Params(60)]
    [Params(2)]
    public int NoOfElementsInMessage { get; set; }

    //[Params(1, 2, 3)]
    //public int ConsumerCount { get; set; }

    private IBus _bus = Bootstrapper.RegisterBus();
    private ServiceProvider _serviceProvider = Bootstrapper.RegisterServices();
    //private ITripDataPublisher _publisher;
    private ITripDataConsumer _consumer;
    private IWorker _consumeWorker;
    private ITripDataHandler _handler;

    private static int launchCounter = 0;
    private int iterationCounter = 0;

    [GlobalSetup]
    public void GlobalSetup()
    {
        launchCounter++;
        Console.WriteLine($"GlobalSetup ({launchCounter})");

        var client = Bootstrapper.RegisterManagementClient();
        //var serviceProvider = Bootstrapper.RegisterServices();
        // delete all queues
        var queues = client.GetQueues();
        foreach (var queue in queues)
        {
            client.DeleteQueue(queue);
        }
    }

    [IterationSetup]
    public void IterationSetup()
    {
        iterationCounter++;
        Console.WriteLine($"IterationSetup ({iterationCounter})");

        // create message generator
        var generator = new MessageGenerator(NoOfMessages, NoOfElementsInMessage);

        var publisher = new TripDataPublisher(_bus);
        _handler = new TripDataHandler(
            _serviceProvider.GetService<IEdgeRepository>(),
            _serviceProvider.GetService<IFEstConsumptionRepository>(),
            _serviceProvider.GetService<IFRecordedTravelRepository>(),
            _serviceProvider.GetService<IWeatherRepository>());

        // create consumer
        var routingKey = $"{iterationCounter}";
        var publishRoutingKey = $"{routingKey}.data";
        var subscribeRoutingKey = $"{routingKey}.#";

        _consumer = new TripDataConsumer(_bus, $"test_{routingKey}", subscribeRoutingKey);
        _consumeWorker = new ConsumeWorker(_consumer, _handler, $"consumer");

        // publish one message to ensure queue is created
        var publishWorker = new PublishWorker(publisher, publishRoutingKey, () => new List<TripDataDto> { new TripDataDto(DateTime.Now, new List<TripDataItemDto>()) });
        publishWorker.Run().Wait();
        _consumeWorker.Run().Wait();

        // create publish worker & publish no. of messages to specified queue
        publishWorker = new PublishWorker(publisher, publishRoutingKey, generator.GenerateTripMessage);
        publishWorker.Run().Wait();

        Thread.Sleep(2000);

        // print status information
        Console.WriteLine(new string('-', 30));
        Console.WriteLine($"Message Count (setup): {NoOfMessages}");
        Console.WriteLine($"Routing Key: {routingKey}");
        Console.WriteLine($"Publish Key: {publishRoutingKey}");
        Console.WriteLine($"Subscribe Key: {subscribeRoutingKey}");
        Console.WriteLine(new string('-', 30));
    }

    [Benchmark]
    public void Benchmark()
    {
        Console.WriteLine("Running benchmarks...");
        // consume all messages from queue
        _consumeWorker.Run().Wait();
    }
}
