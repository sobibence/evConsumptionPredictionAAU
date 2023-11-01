

using System.Diagnostics;
using System.Security.Cryptography;
using EVCP.Domain;
using EVCP.Domain.Models;

namespace EVCP.Simulation
{
    //Singleton
    class SimulationManager
    {
        private int updateFrequencyMs = 1000;
        private int conCurrentCars = 10;
        private int threadWaitFluctuationMs = 100;
        private List<CarThreadClass> carsThreads = new();

        private static readonly object Instancelock = new object();

        private static SimulationManager instance = new SimulationManager();
        public static SimulationManager Instance
        {
            get
            {
                lock (Instancelock)
                {
                    if (instance == null)
                    {
                        instance = new SimulationManager();
                    }
                }
                return instance;
            }
        }

        public RouteManager routeManager { get; private set; }

        private SimulationManager() { }


        public void InitSimulation()
        {
            routeManager = new RouteManager();
            for (int i = 0; i < conCurrentCars; i++)
            {
                CarThreadClass carThread = new CarThreadClass(updateFrequencyMs, threadWaitFluctuationMs, i, routeManager.RequestRoute());
                carsThreads.Add(carThread);
                Console.WriteLine($"Init {i}. car.");
            }
            startSimulation();
            Thread.Sleep(600000);
            stopAllThreads();
        }

        void startSimulation(){
            foreach(CarThreadClass car in carsThreads){
                car.startThread();
            }
        }

        void stopAllThreads(){
            foreach(CarThreadClass car in carsThreads){
                car.stopThread();
            }
            foreach(CarThreadClass car in carsThreads){
                car.stopAndJoin();
            }
        }


        static void Main(string[] args)
        {
            SimulationManager.Instance.InitSimulation();
            //RouteManager routeManager = new RouteManager();
            // List<Edge> edgeList = routeManager.RequestRoute();
            // foreach(Edge edge in edgeList){
            //     Console.WriteLine(edge.ToString());
            // }

        }

        internal void getRouteFromCar(List<TripData> tripDatas, CarThreadClass carThreadClass)
        {
            Console.WriteLine($"Recieved TripData fromCar {carThreadClass.CarId}:");
            foreach(TripData tripData in tripDatas){
                Console.WriteLine(tripData);
            }
        }
    }



}