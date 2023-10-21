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
            day_in_year = random.Next(365),
            minute_in_day = random.Next(60),
            edge_id = random.Next(1, 5),
            vehicle_id = random.Next(1, 5),
            weather_id = random.Next(1, 5),
            energy_consumption_wh = random.Next(100)
        });

        return result;
    }
}
