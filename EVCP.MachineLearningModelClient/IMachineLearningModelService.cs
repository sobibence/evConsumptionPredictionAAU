
namespace EVCP.MachineLearningModelClient
{
    public interface IMachineLearningModelService
    {
        Task<List<decimal>> Predict(List<ModelInput> modelInputs);
    }
}