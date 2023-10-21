using EVCP.DataAccess;
using EVCP.DataAccess.Repositories;
using EVCP.Domain.Models;

namespace EVCP.Domain.Repositories;

public interface IVehicleTripStatusRepository : IBaseRepository<VehicleTripStatus>
{

}

public class VehicleTripStatusRepository : BaseRepository<VehicleTripStatus>, IVehicleTripStatusRepository
{
    private readonly DapperContext _context;

    public VehicleTripStatusRepository(DapperContext context) : base(context)
    {
        _context = context;
    }
}
