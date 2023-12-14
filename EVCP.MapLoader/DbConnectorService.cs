using EVCP.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace EVCP.MapLoader;



public interface IDbConnectorService
{
    public Task QueryAndInsertMapToDb();
}

public class DbConnectorService : IDbConnectorService
{

    private readonly ILogger<DbConnectorService> _logger;
    private readonly IEdgeRepository edgeRepository;
    private readonly INodeRepository nodeRepository;
    private readonly IEdgeInfoRepository edgeInfoRepository;

    public DbConnectorService(
        ILogger<DbConnectorService> logger,
        IEdgeRepository edgeRepository,
        INodeRepository nodeRepository,
        IEdgeInfoRepository edgeInfoRepository
    )
    {
        _logger = logger;
        this.edgeRepository = edgeRepository;
        this.nodeRepository = nodeRepository;
        this.edgeInfoRepository = edgeInfoRepository;
    }



    public async Task QueryAndInsertMapToDb()
    {
        string aalborgRequestString = @"
                [out:json];
                way
                [""highway""]
                (57.0040, 9.8344, 57.0827, 10.0721)
                ->.road;
                .road out geom;
                ";
        Map map = await MapLoaderClass.RequestAndProcessMap(aalborgRequestString);
        bool successful = await edgeInfoRepository.Create(map.EdgeInfos);

        if (successful)
        {
            _logger.LogInformation("Edge Info insert Success!" + DateTime.Now.ToString() +"."+DateTime.Now.Millisecond.ToString());
            bool successfulb = await nodeRepository.Create(map.Nodes);

            if (successfulb)
            {
                _logger.LogInformation("Node insert Success!"+ DateTime.Now.ToString() +"."+DateTime.Now.Millisecond.ToString());
                bool successfulc = await edgeRepository.Create(map.Edges);

                if (successfulc)
                {
                    _logger.LogInformation("Edge insert Success!"+ DateTime.Now.ToString() +"."+DateTime.Now.Millisecond.ToString());
                }
                else
                {
                    _logger.LogWarning("Edge Insert Fail....");
                }
            }
            else
            {
                _logger.LogWarning("Node Insert Fail....");
            }
        }
        else
        {
            _logger.LogWarning("EdgeInfo Insert Fail....");
        }




    }

    



}
