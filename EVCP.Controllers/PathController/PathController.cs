using EVCP.Domain.Helpers;
using EVCP.Domain.Models;
using EVCP.Domain.Repositories;
using EVCP.Domain.Services;
using EVCP.MachineLearningModelClient;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace EVCP.Controllers.PathController
{
    public class PathController : IPathController
    {
        private readonly ILogger<PathController> logger;
        private readonly IEdgeRepository edgeRepository;

        private readonly INodeRepository nodeRepository;

        private readonly IMapConstructionRepository mapConstructionRepository;
        private readonly IVehicleTripStatusRepository vehicleTripStatusRepository;
        private readonly IMachineLearningModelService machineLearningService;
        public PathController(ILogger<PathController> logger,
                              IEdgeRepository edgeRepository,
                              IVehicleTripStatusRepository vehicleTripStatusRepository,
                              IMachineLearningModelService machineLearningService,
                              IMapConstructionRepository mapConstructionRepository,
                              INodeRepository nodeRepository)
        {
            this.logger = logger;
            this.edgeRepository = edgeRepository;
            this.vehicleTripStatusRepository = vehicleTripStatusRepository;
            this.machineLearningService = machineLearningService;
            this.mapConstructionRepository = mapConstructionRepository;
            this.nodeRepository = nodeRepository;
        }

        public async Task<Path> GetBestPathAsync(int vehicleTripStatusId, int startingNodeId, int destinationNodeId)
        {
            Node? startNode = await nodeRepository.GetByIdAsync(startingNodeId);
            Node? endNode = await nodeRepository.GetByIdAsync(destinationNodeId);
            
            if(startNode is null || endNode is null){
                logger.LogError("start or end node was null");
                throw new NullReferenceException();
            }
            logger.LogInformation(startNode.ToString());
            logger.LogInformation(endNode.ToString());
            
            Dictionary<long, Node> subGraph = await mapConstructionRepository.GetConstructedSubGraphASyncNodeDict(startNode, endNode);
            
            logger.LogInformation("Subgraph size: " + subGraph.Count);
            List<Node> nodeList = AStarSearch.FindPath(subGraph[startNode.NodeIdOsm], subGraph[endNode.NodeIdOsm]);
            List<Edge> pathToTravel = AStarSearch.ConvertNodeListToEdgeList(nodeList);
            

            List<ModelInput> modelInputs = new List<ModelInput>();

            foreach (var currEdge in pathToTravel)
            {
                var weatherAtEdge = WeatherDataGenerator.GenerateWeatherData(DateTime.Now); 
                modelInputs.Add(new ModelInput
                {
                    speed = 50,
                    speed_limit = currEdge.EdgeInfo.SpeedLimit,
                    speed_avg_week = 45,
                    speed_avg_time = 45,
                    speed_avg_week_time = 45,
                    speed_avg = 45,
                    seconds = 10,
                    air_temperature = (float)weatherAtEdge.Temperature,
                    wind_direction = weatherAtEdge.WindDirection,
                    wind_speed_ms = (int)Math.Round(weatherAtEdge.WindSpeed),
                    segangle = false,
                    time = weatherAtEdge.At.Minute, // dunno exactly what we need here TODO
                    weekend = weatherAtEdge.At.DayOfWeek == DayOfWeek.Sunday || weatherAtEdge.At.DayOfWeek == DayOfWeek.Saturday,
                    drifting = false,
                    dry = !weatherAtEdge.IsRaining, //not exactly the same, but its what we have
                    fog = weatherAtEdge.Fog > 0.01, // I am just making stuff up here TODO
                    freezing = weatherAtEdge.Temperature <= 0,
                    none = false,
                    snow = weatherAtEdge.IsSnowing,
                    thunder = false,
                    wet = weatherAtEdge.IsRaining, //not exactly the same, but its what we have
                    living_street = currEdge.EdgeInfo.Highway.Equals("living_street"),
                    motorway = currEdge.EdgeInfo.Highway.Equals("motorway"),
                    motorway_link = currEdge.EdgeInfo.Highway.Equals("motorway_link"),
                    primary =currEdge.EdgeInfo.Highway.Equals("primary"),
                    residential = currEdge.EdgeInfo.Highway.Equals("residential"),
                    secondary = currEdge.EdgeInfo.Highway.Equals("secondary"),
                    secondary_link = currEdge.EdgeInfo.Highway.Equals("secondary_link"),
                    service = currEdge.EdgeInfo.Highway.Equals("service"),
                    tertiary = currEdge.EdgeInfo.Highway.Equals("tertiary"),
                    track = currEdge.EdgeInfo.Highway.Equals("track"),
                    trunk = currEdge.EdgeInfo.Highway.Equals("trunk"),
                    trunk_link = currEdge.EdgeInfo.Highway.Equals("trunk_link"),
                    unclassified = currEdge.EdgeInfo.Highway.Equals("unclassified"),
                    unpaved = currEdge.EdgeInfo.Highway.Equals("unpaved")
                });
            }

            var predictedEstimations = await machineLearningService.Predict(modelInputs);
            foreach(var estimation in predictedEstimations ){
                logger.LogInformation(estimation.ToString());

            }
            var edgeIdToCost = new Dictionary<int, double>();

            for (int i = 0; i < pathToTravel.Count; i++)
            {
                edgeIdToCost.Add(pathToTravel[i].Id, predictedEstimations[i]);
            }

            return new Path()
            {
                edgeIdToCost = edgeIdToCost
            };
        }

    }
}
