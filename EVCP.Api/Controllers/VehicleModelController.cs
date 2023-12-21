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
    public async Task<int?> Create()
    {
        var random = new Random();

        var result = await _vehicleModelRepository.Create(new VehicleModel
        {
            AcPower = random.Next(100),
            AvgConsumptionWhKm = random.Next(100),
            BatterySizeWh = random.Next(500),
            DragCoefficient = random.Next(100),
            FrontalSize = random.Next(100),
            Name = Guid.NewGuid().ToString(),
            Power = random.Next(100),
            ProducerId = random.Next(1, 5),
            PtEfficiency = random.Next(100),
            RollingResistance = random.Next(100),
            WeightKg = random.Next(5000),
            Year = random.Next(2000, 2023)
        });

        return result;
    }
}
