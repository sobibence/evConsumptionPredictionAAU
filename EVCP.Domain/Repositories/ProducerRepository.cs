using EVCP.DataAccess;
using EVCP.DataAccess.Repositories;
using EVCP.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EVCP.Domain.Repositories;

public interface IProducerRepository : IBaseRepository<Producer>
{

}

public class ProducerRepository : BaseRepository<Producer>, IProducerRepository
{
    private readonly ILogger<ProducerRepository> _logger;
    private readonly DapperContext _context;

    public ProducerRepository(ILogger<ProducerRepository> logger, DapperContext context) : base(logger, context)
    {
        _logger = logger;
        _context = context;
    }
}
