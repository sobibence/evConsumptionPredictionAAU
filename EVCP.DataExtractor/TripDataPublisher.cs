using EasyNetQ;
using EVCP.Domain;
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
            new(tripData.EdgePercent, tripData.TripId, tripData.Speed, tripData.Acceleration, tripData.Time);
    }

    public interface ITripDataPublisher
    {
        public Task Publish(TripData[] tripData);
    }
}