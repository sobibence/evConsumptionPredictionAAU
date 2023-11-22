using MachineLearningModelClient;

// TODO delete this before merge
namespace Playground
{
    public class program
    {
        public async void main(String[] args)
        {
            var client = new MachineLearningModelService();
            Console.WriteLine("getting prediction...");
            var result = await client.Predict();
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
