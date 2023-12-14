
namespace EVCP.Controllers.PathController
{
    public interface IPathController
    {
        Task<Path> GetBestPathAsync(int vehicleTripStatusId, int startingNodeId, int destinationNodeId);
    }
}