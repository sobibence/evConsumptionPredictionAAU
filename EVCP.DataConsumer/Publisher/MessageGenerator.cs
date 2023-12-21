using EVCP.Domain.Models;
using EVCP.Domain.Repositories;
using EVCP.Dtos;

namespace EVCP.DataConsumer.Publisher;

public class MessageGenerator
{
    private readonly int _noOfMessages;
    private readonly int _noOfElementsInMessage;

    private IEdgeRepository _edgeRepository;
    private IEnumerable<Edge> _edges;

    private readonly Random _random;

    public MessageGenerator(IEdgeRepository? edgeRepository, int noOfMessages = 10, int noOfElementsInMessage = 60)
    {
        _edgeRepository = edgeRepository ?? throw new ArgumentNullException(nameof(edgeRepository));
        _noOfMessages = noOfMessages;
        _noOfElementsInMessage = noOfElementsInMessage;
        _random = new Random();
        _edges = LoadEdges();
    }

    public IEnumerable<ITripDataDto> GenerateTripMessage()
        => Enumerable.Range(0, _noOfMessages).Select(_ => new TripDataDto(DateTime.Now, GenerateTripMessageItem())).ToArray();

    private IEnumerable<ITripDataItemDto> GenerateTripMessageItem()
        => Enumerable.Range(0, _noOfElementsInMessage).Select(i =>
        {
            var randomEdge = GetRandomEdge();

            return new TripDataItemDto(
                edgePercent: _random.Next(100),
                tripId: _random.Next(1, 11),
                speed: _random.Next(200),
                acceleration: _random.Next(-100, 100),
                energyConsumption: _random.Next(-5, 5),
                vehicleId: _random.Next(1, 11),
                time: DateTime.UtcNow,
                weather: new WeatherDto(
                    temperatureCelsius: _random.Next(-10, 40),
                    windKph: _random.Next(100),
                    windDirection: _random.Next(-180, 180),
                    fogPercent: _random.Next(100),
                    rainMm: _random.Next(30)),
                edge: new EdgeDto(
                    startNodeId: randomEdge != null ? randomEdge.StartNodeId : 0,
                    endNodeId: randomEdge != null ? randomEdge.EndNodeId : 0,
                    osmWayId: randomEdge != null ? randomEdge.EdgeInfoId : 0,
                    length: 0,
                    speedLimit: 0,
                    streetName: "",
                    highway: "",
                    surface: ""));
        }).ToArray();

    private IEnumerable<Edge> LoadEdges() =>
        _edgeRepository.GetAsync().Result ?? new List<Edge>();

    private Edge? GetRandomEdge()
    {
        var id = _random.Next(1, _edges.Count());

        return _edges.Where(e => e.Id == id).FirstOrDefault();
    }
}
