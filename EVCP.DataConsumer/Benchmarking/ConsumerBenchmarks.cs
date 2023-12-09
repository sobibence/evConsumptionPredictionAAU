using BenchmarkDotNet.Attributes;
using EasyNetQ;
using EasyNetQ.Management.Client;
using EVCP.DataConsumer.Consumer;
using EVCP.DataConsumer.Publisher;
using EVCP.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace EVCP.DataConsumer.Benchmarking;

[SimpleJob(iterationCount: 3, warmupCount: 0)]
[MemoryDiagnoser]
[MinColumn, MaxColumn, MeanColumn, MedianColumn]
public class ConsumerBenchmarks
{
    //[Params(10, 100, 1000, 10000)]
    [Params(10, 20)]
    public int NoOfMessages { get; set; }

    [Params(60)]
    public int NoOfElementsInMessage { get; set; }

    //[Params(1, 2, 3)]
    //public int ConsumerCount { get; set; }

    private IBus _bus = Bootstrapper.RegisterBus();
    private IEVDataPublisher _publisher;
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
        var serviceProvider = Bootstrapper.RegisterServices();
        // delete all queues
        var queues = client.GetQueues();
        foreach (var queue in queues)
        {
            client.DeleteQueue(queue);
        }

        // create publisher
        var exchangeId = launchCounter.ToString();
        _publisher = new EVDataPublisher(_bus, $"ev_trip_data_{exchangeId}");
        _handler = new TripDataHandler(
            serviceProvider.GetService<IEdgeRepository>(),
            serviceProvider.GetService<IFEstConsumptionRepository>(),
            serviceProvider.GetService<IFRecordedTravelRepository>(),
            serviceProvider.GetService<IWeatherRepository>());
    }

    [IterationSetup]
    public void IterationSetup()
    {
        iterationCounter++;
        Console.WriteLine($"IterationSetup ({iterationCounter})");

        // create message generator
        var generator = new MessageGenerator(NoOfMessages, NoOfElementsInMessage);

        // create consumer
        var routingKey = $"{launchCounter}_{iterationCounter}";
        var queue = $"queue_{routingKey}";
        var consumer = new EVDataConsumer(_bus, queue, _publisher.Exchange, routingKey);
        _consumeWorker = new ConsumeWorker(consumer, _handler, $"consumer {launchCounter} {iterationCounter}");

        // create publish worker & publish no. of messages to specified queue
        var publishWorker = new PublishWorker(_publisher, routingKey, generator.GenerateTripMessage);
        publishWorker.Run().Wait();

        Thread.Sleep(1000);

        // print status information
        Console.WriteLine(new string('-', 30));
        Console.WriteLine($"Message Count (setup): {NoOfMessages}");
        Console.WriteLine($"Queue: {queue}");
        Console.WriteLine($"Messages in Queue: {QueueInfo(queue).MessagesCount}");
        Console.WriteLine(new string('-', 30));
    }

    [Benchmark]
    public void Benchmark()
    {
        // consume all messages from queue
        _consumeWorker.Run().Wait();
    }

    private QueueStats QueueInfo(string queueName)
    {
        var advancedBus = _bus.Advanced;

        return advancedBus.GetQueueStats(queueName);
    }
}
