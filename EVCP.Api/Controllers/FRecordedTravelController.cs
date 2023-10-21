using EVCP.Domain.Models;
using EVCP.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EVCP.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FRecordedTravelController : ControllerBase
{
    private readonly IFRecordedTravelRepository _fRecordedTravelRepository;

    public FRecordedTravelController(IFRecordedTravelRepository fRecordedTravelRepository)
    {
        _fRecordedTravelRepository = fRecordedTravelRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<FactRecordedTravel>> Get()
    {
        var result = await _fRecordedTravelRepository.GetAsync();

        return result;
    }

    [HttpPost]
    public async Task<bool> Create()
    {
        var random = new Random();

        var result = await _fRecordedTravelRepository.Create(new FactRecordedTravel
        {
            acceleration_metre_per_second_squared = random.Next(100),
            edge_id = random.Next(1, 5),
            edge_percent = random.Next(100),
            energy_consumption_wh = random.Next(100),
            speed_km_per_hour = random.Next(200),
            time_epoch = DateTimeOffset.UtcNow,
            vehicle_id = random.Next(1, 5),
            weather_id = random.Next(1, 5)
        });

        return result;
    }
}
