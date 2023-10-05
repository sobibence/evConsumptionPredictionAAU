
using System.Collections;
using System.Xml.Linq;


namespace EVCP.MapLoader;

public class OsmXmlParser
{

    Dictionary<long, Node> nodes = new(); // nodeid is the key
    List<Edge> edges = new(); // 


    public static void ParseXML(string xmlString){
        XDocument osmData = XDocument.Parse(xmlString);
        

    }
}
