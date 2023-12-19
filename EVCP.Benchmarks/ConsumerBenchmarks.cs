using BenchmarkDotNet.Attributes;
using EasyNetQ;
using EasyNetQ.Management.Client;
using EVCP.DataAccess;
using EVCP.DataConsumer.Consumer;
using EVCP.DataConsumer.Publisher;
using EVCP.Domain.Helpers;
using EVCP.Domain.Repositories;
using EVCP.Dtos;
using Microsoft.Extensions.DependencyInjection;

namespace EVCP.Benchmarks;

[SimpleJob(iterationCount: 3, warmupCount: 0)]
[MinColumn, MaxColumn, MeanColumn, MedianColumn]
public class ConsumerBenchmarks
{
    //[Params(10, 100, 1000, 10000)]
    [Params(10)]
    public int NoOfMessages { get; set; }

    [Params(5)]
    public int NoOfElementsInMessage { get; set; }

    //[Params(1, 2, 3)]
    //public int ConsumerCount { get; set; }

    private IBus _bus = Bootstrapper.RegisterBus();
    private ManagementClient _managementClient = Bootstrapper.RegisterManagementClient();
    private ServiceProvider _serviceProvider = Bootstrapper.RegisterServices();
    private SqlScriptRunner _sqlRunner;
    //private ITripDataHandler _handler;
    //private IWorker _consumeWorker;
    private MessageGenerator _generator;

    private int iterationCounter = 0;

    private static string dir = "C:\\Users\\samue\\Desktop\\repos\\evConsumptionPredictionAAU\\EVCP.Benchmarks";

    private Dictionary<string, string[]> _scripts = new Dictionary<string, string[]>
    {
        { "initial", new string[] { $"{dir}/Scripts/drop_index.sql", $"{dir}/Scripts/drop_partitions.sql" } },
        { "index", new string[] { $"{dir}/Scripts/create_index.sql" } },
        { "partition", new string[] { $"{dir}/Scripts/drop_index.sql", $"{dir}/Scripts/create_partitions" } },
        { "index+partition", new string[] { $"{dir}/Scripts/create_index.sql" } },
    };

    [GlobalSetup(Target = nameof(Initial))]
    public void GlobalSetup()
    {
        Console.WriteLine($"Running setup for {nameof(Initial)} benchmark...");

        Setup("initial");
    }

    [IterationSetup]
    public void IterationSetup()
    {
        iterationCounter++;
        Console.WriteLine($"IterationSetup ({iterationCounter})");

        //DeleteQueues();
        Thread.Sleep(3000);

        var handler = new TripDataHandler(
            _serviceProvider.GetService<IEdgeRepository>(),
            _serviceProvider.GetService<IFEstConsumptionRepository>(),
            _serviceProvider.GetService<IFRecordedTravelRepository>(),
            _serviceProvider.GetService<IWeatherRepository>());

        // create keys
        var routingKey = $"{iterationCounter}";
        var publishRoutingKey = $"{routingKey}.data";
        var subscribeRoutingKey = $"{routingKey}.#";

        var consumer = new TripDataConsumer(_bus, $"test_{routingKey}", subscribeRoutingKey);
        var consumeWorker = new ConsumeWorker(consumer, handler, $"consumer");

        // publish and consume one message to ensure queue is created
        var publisher1 = new TripDataPublisher(_bus);
        var publishWorker = new PublishWorker(publisher1, publishRoutingKey, () => new List<TripDataDto> { new TripDataDto(DateTime.Now, new List<TripDataItemDto>()) });
        publishWorker.Run().Wait();
        consumeWorker.Run().Wait();

        // create publish worker and publish no. of messages to specified queue
        var publisher2 = new TripDataPublisher(_bus);
        publishWorker = new PublishWorker(publisher2, publishRoutingKey, _generator.GenerateTripMessage);
        publishWorker.Run().Wait();

        Thread.Sleep(5000);

        // print status information
        Console.WriteLine(new string('-', 30));
        Console.WriteLine($"Message Count (setup): {NoOfMessages}");
        Console.WriteLine($"Routing Key: {routingKey}");
        Console.WriteLine(new string('-', 30));
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        Console.WriteLine($"IterationCleanup ({iterationCounter})");

        Thread.Sleep(10000);
    }

    [Benchmark]
    public async Task Initial()
    {
        Console.WriteLine($"Running {nameof(Initial)} benchmarks({iterationCounter})...");

        var routingKey = $"{iterationCounter}";
        var subscribeRoutingKey = $"{routingKey}.#";

        var handler = new TripDataHandler(
            _serviceProvider.GetService<IEdgeRepository>(),
            _serviceProvider.GetService<IFEstConsumptionRepository>(),
            _serviceProvider.GetService<IFRecordedTravelRepository>(),
            _serviceProvider.GetService<IWeatherRepository>());

        // consume all messages from queue
        var consumer = new TripDataConsumer(_bus, $"test_{routingKey}", subscribeRoutingKey);
        var consumeWorker = new ConsumeWorker(consumer, handler, $"consumer");
        await consumeWorker.Run();
    }

    //[GlobalSetup(Target = nameof(WithEdgeIndex))]
    //public void SetupIndex()
    //{
    //    Console.WriteLine($"Running setup for {nameof(WithEdgeIndex)} benchmark...");

    //    Setup("index");
    //}

    //[Benchmark]
    //public void WithEdgeIndex()
    //{
    //    Console.WriteLine($"Running {nameof(WithEdgeIndex)} benchmarks({iterationCounter})...");

    //    // consume all messages from queue
    //    _consumeWorker.Run().Wait();
    //}

    //[GlobalSetup(Target = nameof(WithFactPartitions))]
    //public void SetupPartitions()
    //{
    //    Console.WriteLine($"Running setup for {nameof(WithFactPartitions)} benchmark...");

    //    Setup();
    //    ExecuteScriptsForBenchmark("partition");
    //}

    //[Benchmark]
    //public void WithFactPartitions()
    //{
    //    Console.WriteLine($"Running {nameof(WithFactPartitions)} benchmarks({iterationCounter})...");

    //    // consume all messages from queue
    //    _consumeWorker.Run().Wait();
    //}

    //[GlobalSetup(Target = nameof(WithEdgeIndexAndFactPartitions))]
    //public void SetupIndexAndParition()
    //{
    //    Console.WriteLine($"Running setup for {nameof(WithEdgeIndexAndFactPartitions)} benchmark...");

    //    Setup();
    //    ExecuteScriptsForBenchmark("index+partition");
    //}

    //[Benchmark]
    //public void WithEdgeIndexAndFactPartitions()
    //{
    //    Console.WriteLine($"Running {nameof(WithEdgeIndexAndFactPartitions)} benchmarks({iterationCounter})...");

    //    // consume all messages from queue
    //    _consumeWorker.Run().Wait();
    //}

    private void DeleteQueues()
    {
        // delete all queues
        var queues = _managementClient.GetQueues();
        foreach (var queue in queues)
        {
            _managementClient.DeleteQueue(queue);
        }
    }

    private void CleanDB()
    {
        var cleanupScript = $"{dir}/Scripts/cleanup_fact_tables.sql";
        _sqlRunner.Run(cleanupScript);
    }

    private void Setup(string key)
    {
        _sqlRunner = new SqlScriptRunner(_serviceProvider.GetService<DapperContext>());

        // create message generator
        _generator = new MessageGenerator(
            _serviceProvider.GetService<IEdgeRepository>(),
            NoOfMessages,
            NoOfElementsInMessage);

        CleanDB();
        ExecuteScriptsForBenchmark(key);
        DeleteQueues();
    }

    private void ExecuteScriptsForBenchmark(string key)
    {
        var scriptsToExecute = _scripts.GetValueOrDefault(key);
        if (scriptsToExecute != null && scriptsToExecute.Length > 0)
        {
            foreach (var script in scriptsToExecute)
            {
                _sqlRunner.Run(script);
            }
        }
    }
}
