using EVCP.DataAccess;
using EVCP.DataAccess.Repositories;
using EVCP.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EVCP.Domain.Repositories;

public interface IVehicleModelRepository : IBaseRepository<VehicleModel>
{

}

public class VehicleModelRepository : BaseRepository<VehicleModel>, IVehicleModelRepository
{
    private readonly ILogger<VehicleModelRepository> _logger;

    public VehicleModelRepository(ILogger<VehicleModelRepository> logger, DapperContext context) : base(logger, context)
    {
        _logger = logger;
    }
}
