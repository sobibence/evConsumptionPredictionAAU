using EVCP.DataAccess;
using EVCP.DataAccess.Repositories;
using EVCP.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EVCP.Domain.Repositories;

public interface INodeRepository : IBaseRepository<Node>
{

}

public class NodeRepository : BaseRepository<Node>, INodeRepository
{
    private readonly ILogger<NodeRepository> _logger;
    private readonly DapperContext _context;

    public NodeRepository(ILogger<NodeRepository> logger, DapperContext context) : base(logger, context)
    {
        _logger = logger;
        _context = context;
    }
}
