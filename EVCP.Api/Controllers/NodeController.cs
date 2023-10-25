using EVCP.Domain.Models;
using EVCP.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EVCP.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NodeController : ControllerBase
{
    private readonly INodeRepository _nodeRepository;

    public NodeController(INodeRepository nodeRepository)
    {
        _nodeRepository = nodeRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<Node>> Get()
    {
        var result = await _nodeRepository.GetAsync();

        return result;
    }

    [HttpPost]
    public async Task<bool> Create()
    {
        var random = new Random();

        var result = await _nodeRepository.Create(new Node
        {
            Latitude = random.Next(-90, 90),
            Longitude = random.Next(-180, 180),
            LongitudeMeters = random.Next(1000),
            OsmNodeId = random.Next(1000),
        });

        return result;
    }
}
