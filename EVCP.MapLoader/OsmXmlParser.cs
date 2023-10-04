
using System.Xml.Linq;


namespace EVCP.MapLoader;

public class OsmXmlParser
{
    public static void ParseXML(string xmlString){
        XDocument osmData = XDocument.Parse(xmlString);


    }
}
