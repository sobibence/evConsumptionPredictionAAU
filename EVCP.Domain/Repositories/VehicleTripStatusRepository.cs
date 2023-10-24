using EVCP.DataAccess;
using EVCP.DataAccess.Repositories;
using EVCP.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EVCP.Domain.Repositories;

public interface IVehicleTripStatusRepository : IBaseRepository<VehicleTripStatus>
{

}

public class VehicleTripStatusRepository : BaseRepository<VehicleTripStatus>, IVehicleTripStatusRepository
{
    private readonly ILogger<VehicleTripStatusRepository> _logger;
    private readonly DapperContext _context;

    public VehicleTripStatusRepository(ILogger<VehicleTripStatusRepository> logger, DapperContext context) : base(logger, context)
    {
        _logger = logger;
        _context = context;
    }
}
