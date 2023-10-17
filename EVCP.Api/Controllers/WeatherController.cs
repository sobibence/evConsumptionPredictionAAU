using EVCP.Domain.Models;
using EVCP.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EVCP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherRepository _weatherRepository;

    public WeatherController(IWeatherRepository weatherRepository)
    {
        _weatherRepository = weatherRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<Weather>> Get()
    {
        var result = await _weatherRepository.GetAsync();

        return result;
    }

    [HttpPost]
    public async Task<bool> Create()
    {
        var random = new Random();

        var result = await _weatherRepository.Create(new Weather
        {
            fog_percent = random.Next(100),
            rain_mm = random.Next(1000),
            temperature_celsius = random.Next(30),
            road_type = road_type.asphalt.ToString(),
            wind_direction = wind_direction.SW.ToString(),
        });

        return result;
    }
}