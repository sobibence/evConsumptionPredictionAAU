
using System.Collections;
using System.Numerics;
using System.Xml.Linq;
using EVCP.Domain.Models;

namespace EVCP.MapLoader;

public class OsmXmlParser
{

    static Dictionary<long, Node> nodeDictionary = new(); // nodeid is the key
    static List<Edge> edges = new(); // 


    public static void ParseXML(string xmlString)
    {
        XDocument osmData = XDocument.Parse(xmlString);
        IEnumerable<XElement> ways = osmData.Descendants("way");
        foreach (XElement way in ways)
        {
            if (way != null)
            {
                ProcessOneWayElement(way);
            }
        }
    }

    private static void ProcessOneWayElement(XElement wayElement)
    {
        long wayId = getLongAttrFromElement(wayElement, "id");
        IEnumerable<XElement> nodes = wayElement.Descendants("nd");
        List<Node> wayListOfNodes = new();
        foreach (XElement nodeElement in nodes)
        {
            if (nodeElement != null)
            {

                long nodeId = getLongAttrFromElement(nodeElement, "reference");
                if (nodeDictionary.ContainsKey(nodeId))
                {
                    wayListOfNodes.Add(nodeDictionary[nodeId]);
                }
                else
                {
                    Node node = new();
                    double lat = getDoubleAttrFromElement(nodeElement, "lat");
                    double lon = getDoubleAttrFromElement(nodeElement, "lon");
                    node.Latitude = lat;
                    node.Longitude = lon;
                    node.NodeIdOsm = nodeId;
                    nodeDictionary[nodeId] = node;
                    wayListOfNodes.Add(nodeDictionary[nodeId]);
                }

            }
        }
        IEnumerable<XElement> tags = wayElement.Descendants("tag");
        long speedLimit = 0;
        try
        {
            string tempString = getKtypeAttributes(tags, "maxspeed");
            if (!string.IsNullOrEmpty(tempString))
                speedLimit = long.Parse(getKtypeAttributes(tags, "maxspeed"));
        }
        catch (FormatException ex)
        {
            Console.WriteLine(ex.Message);
        }
        string streetName = getKtypeAttributes(tags, "streetname");
        string surface = getKtypeAttributes(tags, "surface");
        string highwayType = getKtypeAttributes(tags, "highway");
        //constuct the edges
        if (wayListOfNodes.Count >= 2)
        {
            for (int i = 0; i < wayListOfNodes.Count - 1; i++)
            {
                Edge newEdge = new();
                newEdge.StartNode = wayListOfNodes[i];
                newEdge.EndNode = wayListOfNodes[i + 1];
                wayListOfNodes[i].ListOfConnectedEdges.Add(newEdge);
                wayListOfNodes[i + 1].ListOfConnectedEdges.Add(newEdge);
                edges.Add(newEdge);
            }
        }
    }

    private static string getKtypeAttributes(IEnumerable<XElement> tags, string kValue)
    {
        IEnumerable<XElement> filteredTag = tags.Where(x =>
        {
            return x.Attribute("k") != null ? x.Attribute("k")?.Value.ToString() == kValue : false;
        });
        if (filteredTag is not null && filteredTag.Any() && filteredTag.First() is not null)
        {
            XAttribute? speedLimitattr = filteredTag.First().Attribute("v");
            if (speedLimitattr is not null)
            {
                return speedLimitattr.Value.ToString();
            }
        }
        return "";
    }

    private static long getLongAttrFromElement(XElement element, string attribute)
    {
        XAttribute? tempAttr = element.Attribute(attribute);
        long tempValue = -1;
        if (tempAttr != null)
        {
            try
            {
                tempValue = long.Parse(tempAttr.Value.ToString());
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        return tempValue;
    }

    private static double getDoubleAttrFromElement(XElement element, string attribute)
    {
        XAttribute? tempAttr = element.Attribute(attribute);
        double tempValue = -1;
        if (tempAttr != null)
        {
            try
            {
                tempValue = double.Parse(tempAttr.Value.ToString());
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        return tempValue;
    }
}
