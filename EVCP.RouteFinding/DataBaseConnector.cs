using EVCP.DataAccess;
using EVCP.Domain.Models;
using EVCP.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EVCP.RouteFinding;


public interface IDataBaseConnector
{
    void TestDb();
}


public class DataBaseConnector : IDataBaseConnector
{

    private readonly ILogger<DataBaseConnector> _logger;
    private readonly IEdgeRepository edgeRepository;
    private readonly IFEstConsumptionRepository fEstConsumptionRepository;
    private readonly IFRecordedTravelRepository fRecordedTravelRepository;
    private readonly INodeRepository nodeRepository;
    private readonly IProducerRepository producerRepository;
    private readonly IVehicleModelRepository vehicleModelRepository;
    private readonly IVehicleTripStatusRepository vehicleTripStatusRepository;
    private readonly IWeatherRepository weatherRepository;
    public DataBaseConnector(
        ILogger<DataBaseConnector> logger,
        IEdgeRepository edgeRepository,
        IFEstConsumptionRepository fEstConsumptionRepository,
        IFRecordedTravelRepository fRecordedTravelRepository,
        INodeRepository nodeRepository,
        IProducerRepository producerRepository,
        IVehicleModelRepository vehicleModelRepository,
        IVehicleTripStatusRepository vehicleTripStatusRepository,
        IWeatherRepository weatherRepository
    )
    {
        _logger = logger;
        this.edgeRepository = edgeRepository;
        this.fEstConsumptionRepository = fEstConsumptionRepository;
        this.fRecordedTravelRepository = fRecordedTravelRepository;
        this.nodeRepository = nodeRepository;
        this.producerRepository = producerRepository;
        this.vehicleModelRepository = vehicleModelRepository;
        this.vehicleTripStatusRepository = vehicleTripStatusRepository;
        this.weatherRepository = weatherRepository;
    }

    public async void TestDb()
    {
        Node node = new Node();
        // VehicleTripStatus trip = new VehicleTripStatus
        // {
        //     AdditionalWeightKg = 0,
        //     VehicleMilageMeters = 0,
        //     VehicleId = 1
        // };
        List<Node> list = new()
        {
            node
        };

        var repo = nodeRepository;
        if (repo is null)
        {
            _logger.LogInformation("repo is null");
        }
        else
        {
            await repo.Create(list);
            var asd = await repo.GetByIdAsync(0);
        }
    }
}
