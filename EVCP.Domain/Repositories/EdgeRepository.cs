using EVCP.DataAccess;
using EVCP.DataAccess.Repositories;
using EVCP.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EVCP.Domain.Repositories
{
    public interface IEdgeRepository : IBaseRepository<Edge>
    {

    }

    public class EdgeRepository : BaseRepository<Edge>, IEdgeRepository
    {
        private readonly ILogger<EdgeRepository> _logger;
        private readonly DapperContext _context;

        public EdgeRepository(ILogger<EdgeRepository> logger, DapperContext context) : base(logger, context)
        {
            _logger = logger;
            _context = context;
        }
    }
}
