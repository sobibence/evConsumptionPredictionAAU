using BenchmarkDotNet.Attributes;
using EasyNetQ;
using EVCP.DataConsumer.Consumer;
using EVCP.DataConsumer.Publisher;

namespace EVCP.DataConsumer.Benchmarking;

//[MemoryDiagnoser]
[SimpleJob(iterationCount: 3)]
[MinColumn, MaxColumn, MeanColumn, MedianColumn]
public class BenchmarkingConsumer
{
    [Params(10, 20)]
    public int NoOfMessages { get; set; }

    [Params(5)]
    public int NoOfElementsInMessage { get; set; }

    public int ConsumerCount { get; set; }

    private IBus _bus;
    private IEVDataPublisher _publisher;
    private IWorker _consumeWorker;

    private int launchCounter;
    private int iterationCounter;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _bus = Bootstrapper.RegisterBus();

        launchCounter++;
        var exchangeId = launchCounter.ToString();
        _publisher = new EVDataPublisher(_bus, $"ev_trip_data_{exchangeId}");
    }

    [IterationSetup]
    public void IterationSetup()
    {
        iterationCounter++;
        Console.WriteLine($"IterationSetup ({iterationCounter})");

        var routingKey = $"{launchCounter}_{iterationCounter}";
        var queue = $"queue_{routingKey}";

        _bus.Advanced.QueueDelete(queue);
        var consumer = new EVDataConsumer(_bus, queue, _publisher.Exchange, routingKey);

        var publishWorker = new PublishWorker(_publisher, routingKey, NoOfMessages, NoOfElementsInMessage);
        _consumeWorker = new ConsumeWorker(consumer, $"consumer {launchCounter} {iterationCounter}");

        // fill queue with defined number of messages = NoOfMessages
        publishWorker.Run().Wait();

        Task.Run(() =>
        {
            Console.WriteLine(new string('-', 30));
            Console.WriteLine($"Message Count (setup): {NoOfMessages}");
            Console.WriteLine($"Queue: {queue}");
            Console.WriteLine($"Messages in Queue: {EVDataConsumer.QueueInfo(_bus, queue).MessagesCount}");
            Console.WriteLine(new string('-', 30));
        });
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        Task.Run(() =>
        {
            var routingKey = $"{launchCounter}_{iterationCounter}";
            var queue = $"queue_{routingKey}";

            Console.WriteLine($"Queue: {queue}");
            Console.WriteLine($"Messages in Queue: {EVDataConsumer.QueueInfo(_bus, queue).MessagesCount}");
        });
    }

    [Benchmark]
    public void Benchmark()
    {
        // consume message from queue and measure how long does it take
        _consumeWorker.Run().Wait();
    }
}
