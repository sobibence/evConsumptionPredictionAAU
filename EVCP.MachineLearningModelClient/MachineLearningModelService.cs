using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace EVCP.MachineLearningModelClient
{
    public class MachineLearningModelService : IMachineLearningModelService
    {
        // TODO we are just hardcoding these here, but might want appsettings injection
        private static readonly string API_URL = "localhost:8080"; 
        private static readonly string PREDICT_ENDPOINT = "predict";
        private HttpClient httpClient;

        public MachineLearningModelService()
        {
            this.httpClient = new HttpClient();
        }

        // TODO choose precision. If this is to be used as weights for the edges, we would like a very precise value (decimal)
        // or should it still be float
        public async Task<List<decimal>> Predict(List<ModelInput> inputs)
        {
            string url = API_URL + "/" + PREDICT_ENDPOINT;
            string bodyContent = JsonConvert.SerializeObject(buildJsonArrays(inputs));
            var body = new StringContent(bodyContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(url, body);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                try
                {
                    return JsonConvert.DeserializeObject<List<decimal>>(responseContent);
                }
                catch
                {
                    throw new Exception("Failed to parse API response into expected array of decimals");
                }                   
            }
            else
            {
                throw new Exception($"API request failed with status code {response.StatusCode}");
            }
        }

        // VERY unoptmized, but i was not sure how to properly pass this data into pythons wanted format TODO
        private dynamic[][] buildJsonArrays(List<ModelInput> inputs)
        {
            List<dynamic[]> arrays = new List<dynamic[]>();

            for (int i = 0; i < inputs.Count; i++)
            {
                var array = new dynamic[35];
                array = [
                    inputs[i].speed,
                    inputs[i].speed_limit,
                    inputs[i].speed_avg_week,
                    inputs[i].speed_avg_time,
                    inputs[i].speed_avg_week_time,
                    inputs[i].speed_avg,
                    inputs[i].seconds,
                    inputs[i].air_temperature,
                    inputs[i].wind_direction,
                    inputs[i].wind_speed_ms,
                    inputs[i].segangle ? 1 : 0,
                    inputs[i].time,
                    inputs[i].weekend ? 1 : 0,
                    inputs[i].drifting ? 1 : 0,
                    inputs[i].dry ? 1 : 0,
                    inputs[i].fog ? 1 : 0,
                    inputs[i].freezing ? 1 : 0,
                    inputs[i].none ? 1 : 0,
                    inputs[i].snow ? 1 : 0,
                    inputs[i].thunder ? 1 : 0,
                    inputs[i].wet ? 1 : 0,
                    inputs[i].living_street ? 1 : 0,
                    inputs[i].motorway ? 1 : 0,
                    inputs[i].motorway_link ? 1 : 0,
                    inputs[i].primary ? 1 : 0,
                    inputs[i].residential ? 1 : 0,
                    inputs[i].secondary ? 1 : 0,
                    inputs[i].secondary_link ? 1 : 0,
                    inputs[i].service ? 1 : 0,
                    inputs[i].tertiary ? 1 : 0,
                    inputs[i].track ? 1 : 0,
                    inputs[i].trunk ? 1 : 0,
                    inputs[i].trunk_link ? 1 : 0,
                    inputs[i].unclassified ? 1 : 0,
                    inputs[i].unpaved ? 1 : 0,
                ];
                arrays.Add(array);
            }

            return arrays.ToArray();
        }
    }
}
