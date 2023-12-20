using BenchmarkDotNet.Attributes;
using EVCP.DataAccess;
using EVCP.DataConsumer.Consumer;
using EVCP.DataConsumer.Publisher;
using EVCP.Domain.Helpers;
using EVCP.Domain.Repositories;
using EVCP.Dtos;
using Microsoft.Extensions.DependencyInjection;

namespace EVCP.Benchmarks;

[RPlotExporter]
[MinColumn, MaxColumn, MeanColumn, MedianColumn]
public class TripDataHandlerBenchmarks
{
    [Params(10)]
    public int NoOfMessages { get; set; }

    [Params(60)]
    public int NoOfElementsInMessage { get; set; }

    private ServiceProvider _serviceProvider = Bootstrapper.RegisterServices();
    private SqlScriptRunner _sqlRunner;
    private MessageGenerator _generator;
    private IEnumerable<ITripDataDto> _dataToStore = new List<TripDataDto>();

    private int iterationCounter = 0;

    private static string dir = "C:\\Users\\samue\\Desktop\\repos\\evConsumptionPredictionAAU\\EVCP.Benchmarks";

    private Dictionary<string, string[]> _scripts = new Dictionary<string, string[]>
    {
        { "initial", new string[] { $"{dir}/Scripts/drop_index.sql", $"{dir}/Scripts/drop_partitions.sql" } },
        { "index", new string[] { $"{dir}/Scripts/create_index.sql" } },
        { "partition", new string[] { $"{dir}/Scripts/drop_index.sql", $"{dir}/Scripts/create_partitions.sql" } },
        { "index+partition", new string[] { $"{dir}/Scripts/create_index.sql" } },
    };

    [IterationSetup]
    public void IterationSetup()
    {
        iterationCounter++;
        Console.WriteLine($"IterationSetup ({iterationCounter})");

        _dataToStore = _generator.GenerateTripMessage();
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        Console.WriteLine($"IterationCleanup ({iterationCounter})");
    }

    [GlobalSetup(Target = nameof(Initial))]
    public void GlobalSetup()
    {
        Console.WriteLine($"Running setup for {nameof(Initial)} benchmark...");

        Setup("initial");
    }

    [Benchmark]
    public void Initial()
    {
        Console.WriteLine($"Running {nameof(Initial)} benchmarks({iterationCounter})...");

        var handler = new TripDataHandler(
            _serviceProvider.GetService<IEdgeRepository>(),
            _serviceProvider.GetService<IFEstConsumptionRepository>(),
            _serviceProvider.GetService<IFRecordedTravelRepository>(),
            _serviceProvider.GetService<IWeatherRepository>());

        foreach (var tripData in _dataToStore)
            handler.Handle(tripData);
    }

    [GlobalSetup(Target = nameof(WithEdgeIndex))]
    public void SetupIndex()
    {
        Console.WriteLine($"Running setup for {nameof(WithEdgeIndex)} benchmark...");

        Setup("index");
    }

    [Benchmark]
    public void WithEdgeIndex()
    {
        Console.WriteLine($"Running {nameof(WithEdgeIndex)} benchmarks({iterationCounter})...");

        var handler = new TripDataHandler(
            _serviceProvider.GetService<IEdgeRepository>(),
            _serviceProvider.GetService<IFEstConsumptionRepository>(),
            _serviceProvider.GetService<IFRecordedTravelRepository>(),
            _serviceProvider.GetService<IWeatherRepository>());

        foreach (var tripData in _dataToStore)
            handler.Handle(tripData);
    }

    [GlobalSetup(Target = nameof(WithFactPartitions))]
    public void SetupPartitions()
    {
        Console.WriteLine($"Running setup for {nameof(WithFactPartitions)} benchmark...");

        Setup("partition");
    }

    [Benchmark]
    public void WithFactPartitions()
    {
        Console.WriteLine($"Running {nameof(WithFactPartitions)} benchmarks({iterationCounter})...");

        var handler = new TripDataHandler(
            _serviceProvider.GetService<IEdgeRepository>(),
            _serviceProvider.GetService<IFEstConsumptionRepository>(),
            _serviceProvider.GetService<IFRecordedTravelRepository>(),
            _serviceProvider.GetService<IWeatherRepository>());

        foreach (var tripData in _dataToStore)
            handler.Handle(tripData);
    }

    [GlobalSetup(Target = nameof(WithEdgeIndexAndFactPartitions))]
    public void SetupIndexAndParition()
    {
        Console.WriteLine($"Running setup for {nameof(WithEdgeIndexAndFactPartitions)} benchmark...");

        Setup("index+partition");
    }

    [Benchmark]
    public void WithEdgeIndexAndFactPartitions()
    {
        Console.WriteLine($"Running {nameof(WithEdgeIndexAndFactPartitions)} benchmarks({iterationCounter})...");

        var handler = new TripDataHandler(
            _serviceProvider.GetService<IEdgeRepository>(),
            _serviceProvider.GetService<IFEstConsumptionRepository>(),
            _serviceProvider.GetService<IFRecordedTravelRepository>(),
            _serviceProvider.GetService<IWeatherRepository>());

        foreach (var tripData in _dataToStore)
            handler.Handle(tripData);
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
    }

    private void CleanDB()
    {
        var cleanupScript = $"{dir}/Scripts/cleanup_fact_tables.sql";
        _sqlRunner.Run(cleanupScript);
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
