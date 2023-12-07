
using EVCP.DataAccess;
using EVCP.Domain.Repositories;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace EVCP.MapLoader
{
    public class MapLoaderClass
    {


        public static ServiceProvider? service;
        static async Task Main(string[] args)
        {
            // string aauRequestString = @"
            //     [out:json];
            //     way
            //     [""highway""]
            //     (57, 9.9644, 57.01997, 10.021)
            //     ->.road;
            //     .road out geom;
            //     ";
            // string aalborgRequestString = @"
            //     [out:json];
            //     way
            //     [""highway""]
            //     (57.0040, 9.8344, 57.0827, 10.0721)
            //     ->.road;
            //     .road out geom;
            //     ";


            // Stream response = await RequestMap(aalborgRequestString);
            // OsmJsonParser.ParseAndProcess(response);
            //await SaveToFile(aalborgRequestString);
            // Stream file = File.OpenRead("/home/sobibence/project/evConsumptionPredictionAAU/tmpJson/map.txt");
            // OsmJsonParser.ParseAndProcess(file);
            // file.Close();

            service = RegisterServices();
            IDbConnectorService? dbconnector = service.GetService<IDbConnectorService>();
            if(dbconnector is null){
                throw new NullReferenceException("dbconnector");
            }
            dbconnector.QueryAndInsertMapToDb().Wait();
        }



        public static ServiceProvider RegisterServices()
        {
            if (service is not null)
            {
                return service;
            }
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            var serviceProvider = new ServiceCollection()
                .AddSingleton(provider => new DapperContext(GetConfiguration()))
                .AddLogging(logging =>
                {
                    logging.AddConsole();
                })
                .AddTransient<IEdgeInfoRepository, EdgeInfoRepository>()
                .AddTransient<INodeRepository, NodeRepository>()
                .AddTransient<IEdgeRepository, EdgeRepository>()
                .AddScoped<IDbConnectorService, DbConnectorService>()
                .BuildServiceProvider();
            service = serviceProvider;
            return serviceProvider;
        }

        public static async Task<Map> RequestAndProcessMap(string query)
        {
            Stream response = await RequestMap(query);
            OsmJsonParser.ParseAndProcess(response).Wait();
            return new Map { Nodes = OsmJsonParser.NodeDictionary.Values.ToList(), Edges = OsmJsonParser.Edges, EdgeInfos = OsmJsonParser.EdgeInfos };
        }

        public static async Task<Map> ReadMapFromFile(string filestr = "/home/sobibence/project/evConsumptionPredictionAAU/tmpJson/map.txt")
        {
            Stream file = File.OpenRead(filestr);
            Task jsonTask = OsmJsonParser.ParseAndProcess(file);
            jsonTask.Wait();
            file.Close();
            return new Map { Nodes = OsmJsonParser.NodeDictionary.Values.ToList(), Edges = OsmJsonParser.Edges, EdgeInfos = OsmJsonParser.EdgeInfos };
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

        static async Task SaveToFile(string request)
        {
            Stream response = await RequestMap(request);
            FileStream file = File.Create("/home/sobibence/AAU/1_semester/project/evConsumptionPredictionAAU/tmpJson/map.txt");
            response.CopyTo(file);
            file.Close();
        }


        private static IConfiguration GetConfiguration()
        => new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    }
}



