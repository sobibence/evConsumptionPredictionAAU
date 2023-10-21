using EVCP.DataAccess;
using EVCP.DataAccess.Repositories;
using EVCP.Domain.Models;

namespace EVCP.Domain.Repositories;

public interface IVehicleModelRepository : IBaseRepository<VehicleModel>
{

}

public class VehicleModelRepository : BaseRepository<VehicleModel>, IVehicleModelRepository
{
    private readonly DapperContext _context;

    public VehicleModelRepository(DapperContext context) : base(context)
    {
        _context = context;
    }
}
