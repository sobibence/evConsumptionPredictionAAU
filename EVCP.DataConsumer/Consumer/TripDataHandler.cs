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

        if (recordedTravelList.Count() > 0) await _fRecordedTravelRepository.Create(recordedTravelList.ToList());
        if (estimatedConsumptionList.Count() > 0) await _fEstConsumptionRepository.Create(estimatedConsumptionList.ToList());
    }

    private (IEnumerable<FactRecordedTravel> recordedTravelList, IEnumerable<FactEstimatedConsumption> estimatedConsumptionList) Map(ITripDataDto dto)
    {
        var recordedTravelList = new List<FactRecordedTravel>();
        var estimatedConsumptionList = new List<FactEstimatedConsumption>();

        dto.Data.ToList().ForEach(item =>
        {
            var edge = MapEdge(item.Edge).Result;
            var weatherId = MapWeather(item.Weather).Result;

            if (edge != null && weatherId != null)
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
                    WeatherId = weatherId.Value
                };
                recordedTravelList.Add(recordedTravel);

                var estimatedConsumption = new FactEstimatedConsumption
                {
                    DayInYear = Convert.ToInt16(item.Time.DayOfYear),
                    EdgeId = edge.Id,
                    EstimationType = "record",
                    EnergyConsumptionWh = item.EnergyConsumption,
                    MinuteInDay = Convert.ToInt16(item.Time.Hour * 60 + item.Time.Minute),
                    VehicleId = item.VehicleId,
                    WeatherId = weatherId.Value
                };
                estimatedConsumptionList.Add(estimatedConsumption);
            }
        });

        return (recordedTravelList, estimatedConsumptionList);
    }

    private async Task<Edge?> MapEdge(EdgeDto edgeDto)
    {
        var edge = await _edgeRepository.GetByAttributesAsync(edgeDto.StartNodeId, edgeDto.EndNodeId, edgeDto.OsmWayId);

        if (edge == null)
        {
            // create edge?
        }

        return edge;
    }

    private async Task<int?> MapWeather(WeatherDto weatherDto)
    {
        //var weather = _weatherRepository.GetByMatchingAttributes(weatherDto.TemperatureCelsius,
        //    weatherDto.WindKph, weatherDto.WindDirection, weatherDto.FogPercent, weatherDto.RainMm).Result;

        //if (weather == null)
        //{

        //Weather? weather = null;

        var id = await _weatherRepository.Create(new Weather
        {
            FogPercent = weatherDto.FogPercent,
            RainMm = weatherDto.RainMm,
            Sunshine = 0,
            TemperatureCelsius = weatherDto.TemperatureCelsius,
            WindDirection = weatherDto.WindDirection,
            WindSpeed = weatherDto.WindKph
        });

        //if (created)
        //    weather = await _weatherRepository.GetByMatchingAttributes(weatherDto.TemperatureCelsius,
        //weatherDto.WindKph, weatherDto.WindDirection, weatherDto.FogPercent, weatherDto.RainMm);
        //}

        return id;
    }
}
