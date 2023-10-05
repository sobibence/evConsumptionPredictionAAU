

namespace EVCP.Simulation
{
    //Singleton
    class SimulationManager
    {
        private static SimulationManager instance = new SimulationManager();
        private static SimulationManager Instance{
            get{
                if (instance == null){
                    instance = new SimulationManager();
                }
                return instance;
            }
        }

        private SimulationManager(){}

        
        public void InitSimulation()
        {


        }


        static void Main(string[] args)
        {
            // Display the number of command line arguments.
            Console.WriteLine(args.Length);
            SimulationManager.Instance.InitSimulation();
        }
    }



}