using EVCP.DataAccess;
using EVCP.DataAccess.Repositories;
using EVCP.Domain.Models;

namespace EVCP.Domain.Repositories;

public class NodeRepository : BaseRepository<Node>, INodeRepository
{
    private readonly DapperContext _context;

    public NodeRepository(DapperContext context) : base(context)
    {
        _context = context;
    }
}
