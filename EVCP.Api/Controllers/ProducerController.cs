using EVCP.Domain.Models;
using EVCP.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EVCP.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProducerController : ControllerBase
{
    private readonly IProducerRepository _producerRepository;

    public ProducerController(IProducerRepository producerRepository)
    {
        _producerRepository = producerRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<Producer>> Get()
    {
        var result = await _producerRepository.GetAsync();

        return result;
    }

    [HttpPost]
    public async Task<bool> Create()
    {
        var random = new Random();

        var result = await _producerRepository.Create(new Producer
        {
            Name = Guid.NewGuid().ToString(),
        });

        return result;
    }
}
