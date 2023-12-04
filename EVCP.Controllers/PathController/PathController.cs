using EVCP.Domain.Models;
using EVCP.Domain.Repositories;
using EVCP.Domain.Services;
using EVCP.MachineLearningModelClient;
using System.Security.Cryptography.X509Certificates;

namespace EVCP.Controllers.PathController
{
    public class PathController : IPathController
    {
        private IEdgeRepository edgeRepository;
        private IVehicleTripStatusRepository vehicleTripStatusRepository;
        private IMachineLearningModelService machineLearningService;
        public PathController(IEdgeRepository edgeRepository,
                              IVehicleTripStatusRepository vehicleTripStatusRepository,
                              IMachineLearningModelService machineLearningService)
        {
            this.edgeRepository = edgeRepository;
            this.vehicleTripStatusRepository = vehicleTripStatusRepository;
            this.machineLearningService = machineLearningService;
        }

        public async Task<Path> GetBestPathAsync(int vehicleTripStatusId, int startingNodeId, int destinationNodeId)
        {
            // TODO add some information about the car
            // TODO actually query some paths from database accoring to startingNode and destinationNode
            // TODO fill out information from weatherAPI and others if neccessary
            List<int> wantedEdgeIds = new List<int>() { 1, 2, 3, 4 };

            var vehicleTripStatus = await vehicleTripStatusRepository.GetByIdAsync(vehicleTripStatusId);
            List<Edge> edges = new List<Edge>();
            foreach (var currEdgeId in wantedEdgeIds)
            {
                edges.Add(await edgeRepository.GetByIdAsync(currEdgeId)); //TODO probably transform this into a single query
            }

            List<ModelInput> modelInputs = new List<ModelInput>();

            foreach (var currEdge in edges)
            {
                // TODO Alot of these data is just made up, maybe we could do some clean or accept that its a limitation we do
                var weatherAtEdge = WeatherDataGenerator.GenerateWeatherData(DateTime.Now); //TODO use actual API when we have the key
                modelInputs.Add(new ModelInput
                {
                    speed = 50, // TODO we need the speedlimit here. currEdge.speedLimmit
                    speed_limit = 50, // TODO we need the speedlimit here.currEdge.speedLimmit
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
                    living_street = false,
                    motorway = false,
                    motorway_link = false,
                    primary = true,
                    residential = false,
                    secondary = false,
                    secondary_link = false,
                    service = false,
                    tertiary = false,
                    track = false,
                    trunk = false,
                    trunk_link = false,
                    unclassified = false,
                    unpaved = false,
                });
            }

            var predictedEstimations = await machineLearningService.Predict(modelInputs);

            var edgeIdToCost = new Dictionary<int, decimal>();

            for (int i = 0; i < wantedEdgeIds.Count; i++)
            {
                edgeIdToCost.Add(wantedEdgeIds[i], predictedEstimations[i]);
            }
            // TODO now edgeIdToCost can be used for proper pathfinding. Right now we just return a path with all nodes connected

            return new Path()
            {
                edgeIdToCost = edgeIdToCost
            };
        }


    }
}
