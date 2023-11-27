using BenchmarkDotNet.Attributes;
using EasyNetQ;
using EVCP.DataConsumer.Consumer;
using EVCP.DataConsumer.Publisher;

namespace EVCP.DataConsumer.Benchmarking;

//[MemoryDiagnoser]
[SimpleJob(iterationCount: 3)]
[MinColumn, MaxColumn, MeanColumn, MedianColumn]
public class BenchmarkingDemo
{
    [Params(10, 20)]
    public int NoOfMessages { get; set; }

    [Params(5)]
    public int NoOfElementsInMessage { get; set; }

    public int ConsumerCount { get; set; }

    private IBus _bus;
    private IWorker _consumeWorker;

    private int setupCounter;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _bus = Bootstrapper.RegisterBus();
    }

    [IterationSetup]
    public void IterationSetup()
    {
        Console.WriteLine($"IterationSetup ({++setupCounter})");
        Console.WriteLine(new string('-', 30));

        var publisher = new EVDataPublisher(_bus);
        var consumer = new EVDataConsumer(_bus);

        var publishWorker = new PublishWorker(publisher, NoOfMessages, NoOfElementsInMessage);
        _consumeWorker = new ConsumeWorker(consumer, $"consumer {setupCounter}");

        // fill queue with defined number of messages = NoOfMessages
        publishWorker.Run().Wait();

        //var rabbitMessageCount = GetMessageCount(_bus, "test");

        Console.WriteLine($"Message Count (setup): {NoOfMessages}");
        //Console.WriteLine($"Message Count (rabbitmq): {rabbitMessageCount}");
        Console.WriteLine(new string('-', 30));
    }

    private ulong GetMessageCount(IBus bus, string queueName)
    {
        var advancedBus = bus.Advanced;

        return advancedBus.GetQueueStats(queueName).MessagesCount;
    }

    [Benchmark]
    public void Benchmark()
    {
        // consume message from queue and measure how long does it take
        _consumeWorker.Run().Wait();
    }
}
