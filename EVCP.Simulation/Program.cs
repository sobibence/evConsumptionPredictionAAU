

namespace EVCP.Simulation
{
    //Singleton
    class SimulationManager
    {
        private static SimulationManager instance;

        private SimulationManager(){}

        public static SimulationManager getInstance(){
            if (instance == null){
                instance = new SimulationManager();
            }
            return instance;
        }
        public void InitSimulation()
        {


        }


        static void Main(string[] args)
        {
            // Display the number of command line arguments.
            Console.WriteLine(args.Length);
            SimulationManager.getInstance().InitSimulation();
        }
    }



}