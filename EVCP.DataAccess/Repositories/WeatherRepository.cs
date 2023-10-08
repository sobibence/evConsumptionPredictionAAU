using EVCP.DataAccess.Interfaces;
using EVCP.Domain.Models;

namespace EVCP.DataAccess.Repositories;

public class WeatherRepository : BaseRepository<Weather>, IWeatherRepository
{
    private DbContext _context;

    public WeatherRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}
