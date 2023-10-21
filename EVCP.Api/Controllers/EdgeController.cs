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
            allowed_speed_kmph = random.Next(200),
            average_speed_kmph = random.Next(200),
            start_node_id = random.Next(1, 5),
            end_node_id = random.Next(1, 5),
            inclination_degress = random.Next(-90, 90),
            length_meters = random.Next(1000),
            osm_way_id = random.Next(1000),
        });

        return result;
    }
}
