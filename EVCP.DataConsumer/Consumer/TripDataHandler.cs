using EVCP.Domain.Models;
using EVCP.Domain.Repositories;
using EVCP.Dtos;

namespace EVCP.DataConsumer.Consumer;

public interface ITripDataHandler
{
    public void Handle(ITripDataDto tripDataDto);
}
public class TripDataHandler : ITripDataHandler
{
    private readonly IEdgeRepository _edgeRepository;
    private readonly IFEstConsumptionRepository _fEstConsumptionRepository;
    private readonly IFRecordedTravelRepository _fRecordedTravelRepository;
    private readonly IWeatherRepository _weatherRepository;

    public TripDataHandler(
        IEdgeRepository? edgeRepository,
        IFEstConsumptionRepository? fEstConsumptionRepository,
        IFRecordedTravelRepository? fRecordedTravelRepository,
        IWeatherRepository? weatherRepository)
    {
        _edgeRepository = edgeRepository ?? throw new ArgumentNullException(nameof(edgeRepository));
        _fEstConsumptionRepository = fEstConsumptionRepository ?? throw new ArgumentNullException(nameof(fEstConsumptionRepository));
        _fRecordedTravelRepository = fRecordedTravelRepository ?? throw new ArgumentNullException(nameof(fRecordedTravelRepository));
        _weatherRepository = weatherRepository ?? throw new ArgumentNullException(nameof(weatherRepository));
    }

    public async void Handle(ITripDataDto tripDataDto)
    {
        (var recordedTravelList, var estimatedConsumptionList) = Map(tripDataDto);

        await _fRecordedTravelRepository.Create(recordedTravelList.ToList());
        await _fEstConsumptionRepository.Create(estimatedConsumptionList.ToList());
    }

    private (IEnumerable<FactRecordedTravel> recordedTravelList, IEnumerable<FactEstimatedConsumption> estimatedConsumptionList) Map(ITripDataDto dto)
    {
        var recordedTravelList = new List<FactRecordedTravel>();
        var estimatedConsumptionList = new List<FactEstimatedConsumption>();

        dto.Data.ToList().ForEach(async item =>
        {
            var edge = await MapEdge(item.Edge);
            var weather = await MapWeather(item.Weather);

            if (edge != null && weather != null)
            {
                var recordedTravel = new FactRecordedTravel
                {
                    AccelerationMeterPerSecondSquared = item.Acceleration,
                    EdgeId = edge.Id,
                    EdgePercent = item.EdgePercent,
                    EnergyConsumptionWh = item.EnergyConsumption,
                    SpeedKmph = item.Speed,
                    TimeEpoch = item.Time,
                    TripId = item.TripId,
                    VehicleId = item.VehicleId,
                    WeatherId = weather.Id
                };
                recordedTravelList.Add(recordedTravel);

                var estimatedConsumption = new FactEstimatedConsumption
                {
                    DayInYear = item.Time.DayOfYear,
                    EdgeId = edge.Id,
                    EstimationType = "record",
                    EnergyConsumptionWh = item.EnergyConsumption,
                    MinuteInDay = item.Time.Hour * 60 + item.Time.Minute,
                    VehicleId = item.VehicleId,
                    WeatherId = weather.Id
                };
                estimatedConsumptionList.Add(estimatedConsumption);
            }
        });

        return (recordedTravelList, estimatedConsumptionList);
    }

    private async Task<Edge?> MapEdge(EdgeDto edgeDto)
    {
        var edge = (await _edgeRepository.GetByAsync("EdgeInfoId", edgeDto.OsmWayId)).FirstOrDefault();

        if (edge == null)
        {
            // create edge?
        }

        return edge;
    }

    private async Task<Weather?> MapWeather(WeatherDto weatherDto)
    {
        var weather = await _weatherRepository.GetByMatchingAttributes(weatherDto.TemperatureCelsius,
            weatherDto.WindKph, weatherDto.WindDirection, weatherDto.FogPercent, weatherDto.RainMm);

        if (weather == null)
        {
            var created = await _weatherRepository.Create(new Weather
            {
                FogPercent = weatherDto.FogPercent,
                RainMm = weatherDto.RainMm,
                Sunshine = 0,
                TemperatureCelsius = weatherDto.TemperatureCelsius,
                WindDirection = weatherDto.WindDirection,
                WindSpeed = weatherDto.WindKph
            });

            if (created)
                weather = await _weatherRepository.GetByMatchingAttributes(weatherDto.TemperatureCelsius,
            weatherDto.WindKph, weatherDto.WindDirection, weatherDto.FogPercent, weatherDto.RainMm);
        }

        return weather;
    }
}
