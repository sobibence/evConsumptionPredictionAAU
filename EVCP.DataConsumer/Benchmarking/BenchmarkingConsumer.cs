using BenchmarkDotNet.Attributes;
using EasyNetQ;
using EasyNetQ.Management.Client;
using EVCP.DataConsumer.Consumer;
using EVCP.DataConsumer.Publisher;

namespace EVCP.DataConsumer.Benchmarking;

//[MemoryDiagnoser]
[SimpleJob(iterationCount: 3)]
[MinColumn, MaxColumn, MeanColumn, MedianColumn]
public class BenchmarkingConsumer
{
    [Params(5, 10)]
    public int NoOfMessages { get; set; }

    [Params(3)]
    public int NoOfElementsInMessage { get; set; }

    //public int ConsumerCount { get; set; }

    private IBus _bus = Bootstrapper.RegisterBus();
    private IEVDataPublisher _publisher;
    private IWorker _consumeWorker;

    private static int launchCounter = 0;
    private int iterationCounter = 0;

    [GlobalSetup]
    public void GlobalSetup()
    {
        launchCounter++;
        Console.WriteLine($"GlobalSetup ({launchCounter})");

        var client = Bootstrapper.RegisterManagementClient();
        // delete all queues
        var queues = client.GetQueues();
        foreach (var queue in queues)
        {
            client.DeleteQueue(queue);
        }

        // create publisher
        var exchangeId = launchCounter.ToString();
        _publisher = new EVDataPublisher(_bus, $"ev_trip_data_{exchangeId}");
    }

    [IterationSetup]
    public void IterationSetup()
    {
        iterationCounter++;
        Console.WriteLine($"IterationSetup ({iterationCounter})");

        // create consumer
        var routingKey = $"{launchCounter}_{iterationCounter}";
        var queue = $"queue_{routingKey}";
        var consumer = new EVDataConsumer(_bus, queue, _publisher.Exchange, routingKey);
        _consumeWorker = new ConsumeWorker(consumer, $"consumer {launchCounter} {iterationCounter}");

        // create publish worker & publish no. of messages to specified queue
        var publishWorker = new PublishWorker(_publisher, routingKey, NoOfMessages, NoOfElementsInMessage);
        publishWorker.Run().Wait();

        Thread.Sleep(1000);

        // print status information
        Console.WriteLine(new string('-', 30));
        Console.WriteLine($"Message Count (setup): {NoOfMessages}");
        Console.WriteLine($"Queue: {queue}");
        Console.WriteLine($"Messages in Queue: {QueueInfo(queue).MessagesCount}");
        Console.WriteLine(new string('-', 30));
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        Console.WriteLine($"IterationCleanup ({iterationCounter})");

        var routingKey = $"{launchCounter}_{iterationCounter}";
        var queue = $"queue_{routingKey}";

        // print status information
        Console.WriteLine(new string('-', 30));
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
