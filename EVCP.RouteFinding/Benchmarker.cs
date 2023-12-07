using BenchmarkDotNet.Attributes;
using EVCP.Controllers.PathController;
using EVCP.MachineLearningModelClient;
using Microsoft.Extensions.DependencyInjection;
namespace EVCP.RouteFinding;



[MemoryDiagnoser]
[MinColumn, MaxColumn, MeanColumn, MedianColumn]
public class Benchmarker
{
    public IPathController? pathController;

    [Params(29653)]
    public int StartNodeIDs{get;set;}

    [Params(3112)]
    public int EndNodeIDs{get;set;}

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
        pathController.GetBestPathAsync(0, StartNodeIDs, EndNodeIDs).Wait();
    }

}
