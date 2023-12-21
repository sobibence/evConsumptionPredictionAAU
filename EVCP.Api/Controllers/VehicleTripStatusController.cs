using EVCP.Domain.Models;
using EVCP.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EVCP.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehicleTripStatusController : ControllerBase
{
    private readonly IVehicleTripStatusRepository _vehicleTripStatusRepository;
    public VehicleTripStatusController(IVehicleTripStatusRepository vehicleTripStatusRepository)
    {
        _vehicleTripStatusRepository = vehicleTripStatusRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<VehicleTripStatus>> Get()
    {
        var result = await _vehicleTripStatusRepository.GetAsync();

        return result;
    }

    [HttpPost]
    public async Task<int?> Create()
    {
        var random = new Random();

        var result = await _vehicleTripStatusRepository.Create(new VehicleTripStatus
        {
            AdditionalWeightKg = random.Next(1000),
            VehicleMilageMeters = random.Next(1000000),
            VehicleId = random.Next(1, 5)
        });

        return result;
    }
}
