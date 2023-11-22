using System;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningModelClient
{
    public class MachineLearningModelService : IMachineLearningModelService
    {
        // we are just hardcoding these here for this simple application rather than have some kind of appsettings injection
        private static readonly string API_URL = "localhost:8080"; // TODO Bence tell us which port you are using
        private static readonly string PREDICT_ENDPOINT = "predict";
        private HttpClient httpClient;

        public MachineLearningModelService()
        {
            this.httpClient = new HttpClient();
        }

        // TODO choose precision. If this is to be used as weights for the edges, we would like a very precise value (decimal)
        // or should it still be float
        public async Task<decimal> Predict()
        {
            string url = API_URL + "/" + PREDICT_ENDPOINT;
            var body = new StringContent("[]", Encoding.UTF8, "application/json"); // TODO insert real body

            HttpResponseMessage response = await httpClient.PostAsync(url, body);


            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if (decimal.TryParse(responseContent, out decimal result))
                {
                    return result;
                }
                else
                {
                    throw new Exception("Failed to parse API response as decimal.");
                }
            }
            else
            {
                throw new Exception($"API request failed with status code {response.StatusCode}");
            }
        }
    }
}
