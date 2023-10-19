
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace EVCP.MapLoader
{
    public class MapLoaderClass
    {
        static async Task Main(string[] args)
        {
            // string aalborgRequestString = @"
            //     <osm-script>
            //     <query into=""road"" type=""way"">
            //     <has-kv k=""highway""/>
            //     <bbox-query s=""57.0040"" w=""9.8344"" n=""57.0827"" e=""10.0721""/>
            //     </query>
            //     <print from=""road"" geometry=""full"" limit="""" mode=""body""/>
            //     </osm-script>
            // ";
            string aauRequestString = @"
                [out:json];
                way
                [""highway""]
                (57, 9.9644, 57.01997, 10.021)
                ->.road;
                .road out geom;
                ";
            string aalborgRequestString = @"
                [out:json];
                way
                [""highway""]
                (57.0040, 9.8344, 57.0827, 10.0721)
                ->.road;
                .road out geom;
                ";


            // Stream response = await RequestMap(aalborgRequestString);
            // OsmJsonParser.ParseAndProcess(response);
            //await SaveToFile(aalborgRequestString);
            Stream file = File.OpenRead("/home/sobibence/AAU/1_semester/project/evConsumptionPredictionAAU/tmpJson/map.txt");
            OsmJsonParser.ParseAndProcess(file);
            file.Close();
        }

        public static async Task<Map> RequestAndProcessMap(string query)
        {
            Stream response = await RequestMap(query);
            OsmJsonParser.ParseAndProcess(response).Wait();
            return new Map { Nodes = OsmJsonParser.NodeDictionary.Values.ToList(), Edges = OsmJsonParser.Edges };
        }

        public static async Task<Map> ReadMapFromFile(string filestr = "/home/sobibence/AAU/1_semester/project/evConsumptionPredictionAAU/tmpJson/map.txt"){
            Stream file = File.OpenRead(filestr);
            Task jsonTask = OsmJsonParser.ParseAndProcess(file);
            jsonTask.Wait();
            file.Close();
            return new Map { Nodes = OsmJsonParser.NodeDictionary.Values.ToList(), Edges = OsmJsonParser.Edges };
        }

        static async Task<Stream> RequestMap(string query)
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
                        Stream responseBody = await response.Content.ReadAsStreamAsync();
                        //Console.WriteLine(responseBody);
                        Console.WriteLine("Got response, length:" + responseBody.Length);
                        return responseBody;
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
                return null;
            }
        }

        static async Task SaveToFile(string request){
            Stream response = await RequestMap(request);
            FileStream file = File.Create("/home/sobibence/AAU/1_semester/project/evConsumptionPredictionAAU/tmpJson/map.txt");
            response.CopyTo(file);
            file.Close();
        }
    }
}



