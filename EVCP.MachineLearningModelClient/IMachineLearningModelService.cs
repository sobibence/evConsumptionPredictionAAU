
namespace EVCP.MachineLearningModelClient
{
    public interface IMachineLearningModelService
    {
        Task<List<double>> Predict(List<ModelInput> modelInputs);
    }
}