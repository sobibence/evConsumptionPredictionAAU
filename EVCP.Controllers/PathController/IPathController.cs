
namespace EVCP.Controllers.PathController
{
    public interface IPathController
    {
        Task<Path> GetBestPathAsync(int vehicleTripStatusId, long startingNodeId, long destinationNodeId);
        Task<Path> GetBestPathAsyncWithPgRouting(int vehicleTripStatusId, long startingNodeId, long destinationNodeId);
    }
}