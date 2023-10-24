using EVCP.DataAccess;
using EVCP.DataAccess.Repositories;
using EVCP.Domain.Models;

namespace EVCP.Domain.Repositories;

public interface IWeatherRepository : IBaseRepository<Weather>
{

}

public class WeatherRepository : BaseRepository<Weather>, IWeatherRepository
{
    private readonly DapperContext _context;

    public WeatherRepository(DapperContext context) : base(context)
    {
        _context = context;
    }
}
