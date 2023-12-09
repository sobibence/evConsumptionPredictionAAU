using EVCP.Dtos;

namespace EVCP.DataConsumer.Publisher;

public class MessageGenerator
{
    private readonly int _noOfMessages;
    private readonly int _noOfElementsInMessage;

    private readonly Random _random;

    public MessageGenerator(int noOfMessages = 10, int noOfElementsInMessage = 60)
    {
        _noOfMessages = noOfMessages;
        _noOfElementsInMessage = noOfElementsInMessage;
        _random = new Random();
    }

    public IEnumerable<ITripDataDto> GenerateTripMessage()
        => Enumerable.Range(0, _noOfMessages).Select(_ => new TripDataDto(DateTime.Now, GenerateTripMessageItem())).ToArray();

    private IEnumerable<ITripDataItemDto> GenerateTripMessageItem()
        => Enumerable.Range(0, _noOfElementsInMessage).Select(i => new TripDataItemDto(
            edgePercent: _random.Next(100),
            tripId: 1,
            speed: _random.Next(200),
            acceleration: _random.Next(-100, 100),
            energyConsumption: _random.Next(-5, 5),
            vehicleId: 1,
            time: DateTime.UtcNow,
            weather: new WeatherDto(
                temperatureCelsius: _random.Next(-20, 40),
                windKph: _random.Next(100),
                windDirection: _random.Next(-180, 180),
                fogPercent: _random.Next(100),
                rainMm: _random.Next(30)),
            edge: new EdgeDto(
                startNodeId: 0,
                endNodeId: 0,
                osmWayId: _random.Next(1, 6),
                length: 0,
                speedLimit: 0,
                streetName: "",
                highway: "",
                surface: ""))).ToArray();
}
