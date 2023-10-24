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
            FogPercent = random.Next(100),
            RainMm = random.Next(1000),
            TemperatureCelsius = random.Next(30),
            RoadType = road_type.asphalt,
            RoadQuality = random.Next(100),
            SunshineWM = random.Next(100),
            WindDirectionDegrees = random.Next(360),
            WindKmPh = random.Next(100)
        });

        return result;
    }
}