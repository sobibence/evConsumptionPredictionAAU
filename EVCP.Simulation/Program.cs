using EVCP.DataPublisher;
using EVCP.Domain;

namespace EVCP.Simulation
{
    //Singleton
    public class SimulationManager
    {
        private readonly int updateFrequencyMs = 1000;
        private readonly int conCurrentCars = 1;
        private readonly int threadWaitFluctuationMs = 100;
        private List<CarThreadClass> carsThreads = new();

        private static readonly object Instancelock = new();

        private static SimulationManager instance = new();
        public static SimulationManager Instance
        {
            get
            {
                lock (Instancelock)
                {
                    instance ??= new SimulationManager();
                }
                return instance;
            }
        }

        public RouteManager RouteManager { get; private set; }

        private SimulationManager() { }

        private ITripDataPublisher _tripDataPublisher;

        public void InitSimulation()
        {
            var bus = Bootstrapper.RegisterBus();
            _tripDataPublisher = new TripDataPublisher(bus);

            RouteManager = new RouteManager();
            for (int i = 0; i < conCurrentCars; i++)
            {
                CarThreadClass carThread = new(updateFrequencyMs, threadWaitFluctuationMs, i, RouteManager.RequestRoute());
                carsThreads.Add(carThread);
                Console.WriteLine($"Init {i}. car.");
            }
            StartSimulation();
            Thread.Sleep(600000);
            StopAllThreads();
        }

        void StartSimulation()
        {
            foreach (CarThreadClass car in carsThreads)
            {
                car.startThread();
            }
        }

        void StopAllThreads()
        {
            foreach (CarThreadClass car in carsThreads)
            {
                car.stopThread();
            }
            foreach (CarThreadClass car in carsThreads)
            {
                car.stopAndJoin();
            }
        }


        static void Main(string[] args)
        {
            Instance.InitSimulation();
            //RouteManager routeManager = new RouteManager();
            // List<Edge> edgeList = routeManager.RequestRoute();
            // foreach(Edge edge in edgeList){
            //     Console.WriteLine(edge.ToString());
            // }

        }

        internal void GetRouteFromCar(List<TripData> tripDatas, CarThreadClass carThreadClass)
        {
            Console.WriteLine($"Recieved TripData fromCar {carThreadClass.CarId}:");
            foreach (TripData tripData in tripDatas)
            {
                Console.WriteLine(tripData);
            }
        }
    }
}