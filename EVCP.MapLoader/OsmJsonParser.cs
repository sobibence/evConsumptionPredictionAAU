﻿
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EVCP.Domain.Models;

namespace EVCP.MapLoader;

public class OsmJsonParser
{
    static Dictionary<long, Node> nodeDictionary = new(); // nodeid is the key
    static List<Edge> edges = new(); // 

    static List<EdgeInfo> edgeInfos = new();

    public static Dictionary<long, Node> NodeDictionary { get { return nodeDictionary; } }

    public static List<Edge> Edges { get { return edges; } }

    public static List<EdgeInfo> EdgeInfos { get { return edgeInfos; } }

    private static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        IgnoreNullValues = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    static OsmJsonParser()
    {
        _serializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    }
    public static async Task<OverpassApiResponse?> Deserialize(Stream jsonStream)
    {
        return await JsonSerializer.DeserializeAsync<OverpassApiResponse>(jsonStream, _serializerOptions);
    }

    public class OverpassApiResponse
    {
        public Element[] Elements { get; set; }
    }

    public class Element
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public Tags tags { get; set; }

        public List<long> Nodes { get; set; }
        public List<Coordinates> Geometry { get; set; }
    }

    public class Coordinates
    {
        public double lat { get; set; }
        public double lon { get; set; }

    }

    public class Tags
    {
        // Define properties for any tags you want to extract from the response
        private string _name;
        public string name
        {
            get
            {
                if (_name == null)
                {
                    return "";
                }
                else
                {
                    return _name;
                }
            }
            set { _name = value; }
        }

        private string _highway;
        public string highway
        {
            get
            {
                if (_highway == null)
                {
                    return "";
                }
                else
                {
                    return _highway;
                }
            }
            set { _highway = value; }
        }

        public string _maxspeed;
        public string maxspeed
        {
            get
            {
                if (_maxspeed == null)
                {
                    return "0";
                }
                else
                {
                    return _maxspeed;
                }
            }
            set { _maxspeed = value; }
        }

        public string _surface;
        public string surface
        {
            get
            {
                if (_surface == null)
                {
                    return "";
                }
                else
                {
                    return _surface;
                }
            }
            set { _surface = value; }
        }

        // Add more properties as needed
    }

    public static void ProcessJson(OverpassApiResponse? apiResponse)
    {
        if (apiResponse is null)
        {
            return;
        }
        foreach (Element element in apiResponse.Elements)
        {
            if (element.Type == "way")
            {

                if (element.Nodes.Count == element.Geometry.Count)
                {
                    List<Node> wayListOfNodes = new();
                    if (element.Nodes.Count >= 2)
                    {
                        for (int i = 0; i < element.Nodes.Count; i++)
                        {
                            if (nodeDictionary.ContainsKey(element.Nodes[i]))
                            {
                                wayListOfNodes.Add(nodeDictionary[element.Nodes[i]]);
                            }
                            else
                            {
                                Node node = new()
                                {
                                    Latitude = element.Geometry[i].lat,
                                    Longitude = element.Geometry[i].lon,
                                    NodeIdOsm = element.Nodes[i]
                                };
                                nodeDictionary[node.NodeIdOsm] = node;
                                wayListOfNodes.Add(node);
                            }
                        }
                    }
                    EdgeInfo edgeInfo = new()
                    {
                        Highway = element.tags.highway,
                        Surface = element.tags.surface,
                        SpeedLimit = int.Parse(element.tags.maxspeed),
                        StreetName = element.tags.name,
                        OsmWayId = element.Id
                    };
                    for (int j = 0; j < wayListOfNodes.Count - 1; j++)
                    {
                        Edge newEdge = new()
                        {
                            StartNode = wayListOfNodes[j],
                            EndNode = wayListOfNodes[j + 1],
                            EdgeInfo = edgeInfo
                        };
                        Edge alternateEdge = new()
                        {
                            EndNode = wayListOfNodes[j],
                            StartNode = wayListOfNodes[j + 1],
                            EdgeInfo = edgeInfo
                        };
                        wayListOfNodes[j].ListOfConnectedEdges.Add(newEdge);
                        wayListOfNodes[j + 1].ListOfConnectedEdges.Add(newEdge);
                        wayListOfNodes[j].ListOfConnectedNodes.Add(wayListOfNodes[j + 1]);
                        wayListOfNodes[j + 1].ListOfConnectedNodes.Add(wayListOfNodes[j]);

                        edges.Add(newEdge);
                        edges.Add(alternateEdge);

                    }
                    edgeInfos.Add(edgeInfo);
                }
                else
                {
                    Console.WriteLine("This should not happen.");
                }
            }
        }
    }

    internal static async Task ParseAndProcess(Stream stream)
    {
        OverpassApiResponse? response = await Deserialize(stream);
        ProcessJson(response);
    }
}
