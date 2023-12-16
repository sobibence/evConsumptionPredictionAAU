using EVCP.DataAccess;
using EVCP.DataAccess.Repositories;
using EVCP.Domain.Models;
using Microsoft.Extensions.Logging;


namespace EVCP.Domain.Repositories;

public interface IEdgeInfoRepository : IBaseRepository<EdgeInfo>
{

}

public class EdgeInfoRepository : BaseRepository<EdgeInfo>, IEdgeInfoRepository
{
    private readonly ILogger<EdgeInfoRepository> _logger;

    public EdgeInfoRepository(ILogger<EdgeInfoRepository> logger, DapperContext context) : base(logger, context)
    {
        _logger = logger;
    }
}
