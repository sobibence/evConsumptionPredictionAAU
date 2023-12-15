using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using EVCP.Controllers.PathController;
using EVCP.MachineLearningModelClient;
using Microsoft.Extensions.DependencyInjection;
namespace EVCP.RouteFinding;



[MemoryDiagnoser]
[SimpleJob(warmupCount: 1, iterationCount: 5, launchCount: 2)]
[MinColumn, MaxColumn, MeanColumn, MedianColumn]
public class Benchmarker
{
    public IPathController? pathController;

    [Params(352924816L)]
    public long StartNodeIDs{get;set;}

    [Params(828449218L,279727728L,338760520L,1709787516L,5269743916L)]
    public long EndNodeIDs{get;set;}

    [GlobalSetup]
    public void GlobalSetup(){
        IServiceProvider serviceProvider = Program.RegisterServices();
        
        pathController = serviceProvider.GetService<IPathController>();
        if(pathController is null){
            throw new NullReferenceException("pathController is null");
        }
    }

    [Benchmark]
    public void FullRouteFinding(){
        pathController.GetBestPathAsyncWithPgRouting(0, StartNodeIDs, EndNodeIDs).Wait();
    }

}
