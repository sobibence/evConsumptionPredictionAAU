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

}
