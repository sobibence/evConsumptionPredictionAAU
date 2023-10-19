

using System.Diagnostics;

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

        private SimulationManager() { }


        public void InitSimulation()
        {

            for (int i = 0; i < conCurrentCars; i++)
            {
                CarThreadClass carThread = new CarThreadClass(updateFrequencyMs, threadWaitFluctuationMs, i);
                carsThreads.Add(carThread);
            }
            startSimulation();
            Thread.Sleep(10000);
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
            // Display the number of command line arguments.
            Console.WriteLine(args.Length);
            //SimulationManager.Instance.InitSimulation();
            RouteManager routeManager = new RouteManager();
            routeManager.RequestRoute();
        }
    }



}