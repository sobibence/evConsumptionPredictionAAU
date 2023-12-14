using EVCP.Domain.Models;
using EVCP.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EVCP.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FEstConsumptionController : ControllerBase
{
    private readonly IFEstConsumptionRepository _fEstConsumptionRepository;

    public FEstConsumptionController(IFEstConsumptionRepository fEstConsumptionRepository)
    {
        _fEstConsumptionRepository = fEstConsumptionRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<FactEstimatedConsumption>> Get()
    {
        var result = await _fEstConsumptionRepository.GetAsync();

        return result;
    }

    [HttpPost]
    public async Task<bool> Create()
    {
        var random = new Random();

        var result = await _fEstConsumptionRepository.Create(new FactEstimatedConsumption
        {
            DayInYear = Convert.ToInt16(random.Next(365)),
            MinuteInDay = Convert.ToInt16(random.Next(60)),
            EdgeId = random.Next(1, 5),
            VehicleId = 1,
            WeatherId = 130,
            EnergyConsumptionWh = random.Next(100),
            EstimationType = "record",
        });

        return result;
    }
}
