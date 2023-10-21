using EVCP.Domain.Models;
using EVCP.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EVCP.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehicleModelController : ControllerBase
{
    private readonly IVehicleModelRepository _vehicleModelRepository;

    public VehicleModelController(IVehicleModelRepository vehicleModelRepository)
    {
        _vehicleModelRepository = vehicleModelRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<VehicleModel>> Get()
    {
        var result = await _vehicleModelRepository.GetAsync();

        return result;
    }

    [HttpPost]
    public async Task<bool> Create()
    {
        var random = new Random();

        var result = await _vehicleModelRepository.Create(new VehicleModel
        {
            ac_power = random.Next(100),
            avg_consumption_wh_km = random.Next(100),
            battery_size_wh = random.Next(500),
            drag_coefficient = random.Next(100),
            frontal_size = random.Next(100),
            name = Guid.NewGuid().ToString(),
            power = random.Next(100),
            producer_id = random.Next(1, 5),
            pt_effeciency = random.Next(100),
            rolling_resistance = random.Next(100),
            weight_kg = random.Next(5000),
            year = random.Next(2000, 2023)
        });

        return result;
    }
}
