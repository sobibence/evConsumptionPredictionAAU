using EVCP.Domain.Repositories;
using EVCP.Dtos;

namespace EVCP.DataConsumer.Consumer;

public interface ITripDataHandler
{
    public Task<bool> Handle(ITripDataDto tripData);
}
public class TripDataHandler : ITripDataHandler
{
    private readonly IFEstConsumptionRepository _fEstConsumptionRepository;
    private readonly IFRecordedTravelRepository _fRecordedTravelRepository;
    private readonly IWeatherRepository _weatherRepository;

    public TripDataHandler(
        IFEstConsumptionRepository fEstConsumptionRepository,
        IFRecordedTravelRepository fRecordedTravelRepository,
        IWeatherRepository weatherRepository)
    {
        _fEstConsumptionRepository = fEstConsumptionRepository;
        _fRecordedTravelRepository = fRecordedTravelRepository;
        _weatherRepository = weatherRepository;
    }

    public async Task<bool> Handle(ITripDataDto tripData)
    {
        return await Task.FromResult(false);
    }
}
