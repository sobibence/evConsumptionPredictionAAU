using EVCP.DataAccess;
using EVCP.DataAccess.Repositories;
using EVCP.Domain.Models;

namespace EVCP.Domain.Repositories;

public interface IFEstConsumptionRepository : IBaseRepository<FactEstimatedConsumption>
{

}

public class FEstConsumptionRepository : BaseRepository<FactEstimatedConsumption>, IFEstConsumptionRepository
{
    private readonly DapperContext _context;

    public FEstConsumptionRepository(DapperContext context) : base(context)
    {
        _context = context;
    }
}
