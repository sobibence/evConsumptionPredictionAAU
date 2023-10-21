using EVCP.DataAccess;
using EVCP.DataAccess.Repositories;
using EVCP.Domain.Models;

namespace EVCP.Domain.Repositories;

public interface IFRecordedTravelRepository : IBaseRepository<FactRecordedTravel>
{

}

public class FRecordedTravelRepository : BaseRepository<FactRecordedTravel>, IFRecordedTravelRepository
{
    private readonly DapperContext _context;

    public FRecordedTravelRepository(DapperContext context) : base(context)
    {
        _context = context;
    }
}
