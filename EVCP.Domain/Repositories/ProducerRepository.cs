using EVCP.DataAccess;
using EVCP.DataAccess.Repositories;
using EVCP.Domain.Models;

namespace EVCP.Domain.Repositories;

public interface IProducerRepository : IBaseRepository<Producer>
{

}

public class ProducerRepository : BaseRepository<Producer>, IProducerRepository
{
    private readonly DapperContext _context;

    public ProducerRepository(DapperContext context) : base(context)
    {
        _context = context;
    }
}
