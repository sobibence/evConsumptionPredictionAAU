using EVCP.MachineLearningModelClient;

// TODO delete this before merge
namespace Playground
{
    public class Program
    {
        public async void main(String[] args)
        {
            //var app = SetupServices(args);
            var client = new MachineLearningModelService();
            Console.WriteLine("getting prediction...");
            //var result = await client.Predict();
            //Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
