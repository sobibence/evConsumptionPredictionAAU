using EVCP.Domain.Models;
using EVCP.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EVCP.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EdgeController : ControllerBase
{
    private readonly IEdgeRepository _edgeRepository;

    public EdgeController(IEdgeRepository edgeRepository)
    {
        _edgeRepository = edgeRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<Edge>> Get()
    {
        var result = await _edgeRepository.GetAsync();

        return result;
    }

    [HttpPost]
    public async Task<bool> Create()
    {
        var random = new Random();

        var result = await _edgeRepository.Create(new Edge
        {
            AllowedSpeedKmph = random.Next(200),
            AverageSpeedKmph = random.Next(200),
            StartNodeId = random.Next(1, 5),
            EndNodeId = random.Next(1, 5),
            InclinationDegrees = random.Next(-90, 90),
            LengthMeters = random.Next(1000),
            OsmWayId = random.Next(1000),
        });

        return result;
    }
}
