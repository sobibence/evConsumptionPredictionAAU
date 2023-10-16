using EVCP.DataAccess;
using EVCP.Domain.Models;

namespace EVCP.Domain.Repositories;

public class EdgeRepository : BaseRepository<Edge>, IEdgeRepository
{
    private readonly DapperContext _context;

    public EdgeRepository(DapperContext context) : base(context)
    {
        _context = context;
    }
}
