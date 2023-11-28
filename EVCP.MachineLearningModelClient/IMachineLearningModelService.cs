
namespace MachineLearningModelClient
{
    public interface IMachineLearningModelService
    {
        Task<decimal> Predict();
    }
}