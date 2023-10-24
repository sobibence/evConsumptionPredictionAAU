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
            AccelerationMeterPerSecondSquared = random.Next(100),
            EdgeId = random.Next(1, 5),
            EdgePercent = random.Next(100),
            EnergyConsumptionWh = random.Next(100),
            SpeedKmph = random.Next(200),
            TimeEpoch = DateTimeOffset.UtcNow,
            VehicleId = random.Next(1, 5),
            WeatherId = random.Next(1, 5)
        });

        return result;
    }
}
