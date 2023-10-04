
using System;
using System.Net.Http;
using System.Threading.Tasks;


namespace EVCP.MapLoader
{
    class MapLoader
    {
        static async Task Main(string[] args){
            await RequestMap(@"
                <osm-script>
                <query into=""road"" type=""way"">
                <has-kv k=""highway""/>
                <bbox-query s=""57.0040"" w=""9.8344"" n=""57.0827"" e=""10.0721""/>
                </query>
                <print from=""road"" geometry=""full"" limit="""" mode=""body""/>
                </osm-script>
            ");

        }

        static async Task RequestMap(string query)
        {
        // Encode the query for use in a URL
        string encodedQuery = Uri.EscapeDataString(query);

            // Set the Overpass API endpoint
            string overpassUrl = "https://overpass-api.de/api/interpreter?data=" + encodedQuery;

            // Create an instance of HttpClient
            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Send a GET request to the Overpass API
                    HttpResponseMessage response = await httpClient.GetAsync(overpassUrl);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read and display the response content
                        string responseBody = await response.Content.ReadAsStringAsync();
                        //Console.WriteLine(responseBody);
                        Console.WriteLine(responseBody.Length);
                    }
                    else
                    {
                        Console.WriteLine("Error: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}



