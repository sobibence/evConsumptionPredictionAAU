using EVCP.DataAccess;
using EVCP.DataAccess.Repositories;
using EVCP.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EVCP.Domain.Repositories;

public interface IWeatherRepository : IBaseRepository<Weather>
{

}

public class WeatherRepository : BaseRepository<Weather>, IWeatherRepository
{
    private readonly ILogger<WeatherRepository> _logger;
    private readonly DapperContext _context;

    public WeatherRepository(ILogger<WeatherRepository> logger, DapperContext context) : base(logger, context)
    {
        _logger = logger;
        _context = context;
    }
}
