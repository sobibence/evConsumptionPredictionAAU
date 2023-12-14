using EasyNetQ;
using EVCP.Domain;
using EVCP.Domain.Models;
using EVCP.Domain.Services;
using EVCP.Dtos;

namespace EVCP.DataPublisher
{
    public class TripDataPublisher : ITripDataPublisher
    {
        private readonly IBus _bus;

        public TripDataPublisher(IBus bus)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        public async Task Publish(TripData[] tripData)
        {
            var mapped = Map(tripData);
            await _bus.PubSub.PublishAsync((ITripDataDto)mapped);
        }

        private static TripDataDto Map(TripData[] tripData) =>
            new(
                sourceTimestamp: DateTime.Now,
                data: tripData.Select(Map)
                );

        private static TripDataItemDto Map(TripData tripData) =>
            new(tripData.EdgePercent,
                tripData.TripId,
                tripData.Speed,
                tripData.Acceleration,
                tripData.Time,
                tripData.EnergyConsumption,
                tripData.VehicleId,
                Map(WeatherDataGenerator.GenerateWeatherData(tripData.Time)),
                Map(tripData.Edge));

        private static WeatherDto Map(WeatherData weatherData) =>
            new(
                temperatureCelsius: weatherData.Temperature,
                windKph: weatherData.WindSpeed,
                windDirection: weatherData.WindDirection,
                fogPercent: weatherData.Fog,
                rainMm: weatherData.Precipitation
                );

        private static EdgeDto Map(Edge edge) =>
            new(
                startNodeId: edge.StartNodeId,
                endNodeId: edge.EndNodeId,
                osmWayId: edge.EdgeInfo.OsmWayId,
                length: edge.Length,
                speedLimit: edge.EdgeInfo.SpeedLimit,
                streetName: edge.EdgeInfo.StreetName,
                highway: edge.EdgeInfo.Highway,
                surface: edge.EdgeInfo.Surface
                );
    }

    public interface ITripDataPublisher
    {
        public Task Publish(TripData[] tripData);
    }
}