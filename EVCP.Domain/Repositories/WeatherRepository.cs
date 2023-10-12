using EVCP.Domain.Models;

namespace EVCP.Domain.Repositories;

public class WeatherRepository : BaseRepository<Weather>, IWeatherRepository
{
    private DapperContext _context;

    public WeatherRepository(DapperContext context) : base(context)
    {
        _context = context;
    }
}
