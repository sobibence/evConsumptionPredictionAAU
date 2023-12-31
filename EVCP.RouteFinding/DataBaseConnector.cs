﻿using System.Dynamic;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using EVCP.DataAccess;
using EVCP.Domain.Models;
using EVCP.Domain.Repositories;
using EVCP.MapLoader;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EVCP.RouteFinding;


public interface IDataBaseConnector
{
    void QueryAndInsertMapToDb();
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
    private readonly IEdgeInfoRepository edgeInfoRepository;

    private readonly IMapConstructionRepository mapConstructionRepository;
    public DataBaseConnector(
        ILogger<DataBaseConnector> logger,
        IEdgeRepository edgeRepository,
        IFEstConsumptionRepository fEstConsumptionRepository,
        IFRecordedTravelRepository fRecordedTravelRepository,
        INodeRepository nodeRepository,
        IProducerRepository producerRepository,
        IVehicleModelRepository vehicleModelRepository,
        IVehicleTripStatusRepository vehicleTripStatusRepository,
        IWeatherRepository weatherRepository,
        IEdgeInfoRepository edgeInfoRepository,
        IMapConstructionRepository mapConstructionRepository
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
        this.edgeInfoRepository = edgeInfoRepository;
        this.mapConstructionRepository = mapConstructionRepository;
    }



    public async void QueryAndInsertMapToDb()
    {
        // string aalborgRequestString = @"
        //         [out:json];
        //         way
        //         [""highway""]
        //         (57.0040, 9.8344, 57.0827, 10.0721)
        //         ->.road;
        //         .road out geom;
        //         ";
        // Map map = await MapLoaderClass.RequestAndProcessMap(aalborgRequestString);
        Map map = await MapLoaderClass.ReadMapFromFile();
        // HashSet<String> surface = new();
        // foreach (EdgeInfo edgeInfo in map.EdgeInfos){
        //     surface.Add(edgeInfo.Highway);

        // }

        // foreach(String surfacestr in surface){
        //     Console.WriteLine($"'{surfacestr}',");
        // }
        bool successful = await edgeInfoRepository.Create(map.EdgeInfos);

        if (successful)
        {
            _logger.LogInformation("Edge Info insert Success!" + DateTime.Now.ToString() +"."+DateTime.Now.Millisecond.ToString());
            bool successfulb = await nodeRepository.Create(map.Nodes);

            if (successfulb)
            {
                _logger.LogInformation("Node insert Success!"+ DateTime.Now.ToString() +"."+DateTime.Now.Millisecond.ToString());
                bool successfulc = await edgeRepository.Create(map.Edges);

                if (successfulc)
                {
                    _logger.LogInformation("Edge insert Success!"+ DateTime.Now.ToString() +"."+DateTime.Now.Millisecond.ToString());
                }
                else
                {
                    _logger.LogWarning("Edge Insert Fail....");
                }
            }
            else
            {
                _logger.LogWarning("Node Insert Fail....");
            }
        }
        else
        {
            _logger.LogWarning("EdgeInfo Insert Fail....");
        }




    }

    



}
