using EVCP.Domain.Models;

namespace EVCP.Domain.Helpers;

public class GpsDistanceCalculator
{
    //shamelessly stolen from chatgpt
    public static double CalculateDistance(Node point1, Node point2)
    {
        double earthRadius = 6371000; // Earth's radius in meters 

        double distance = earthRadius * CalculateDistanceInRadians(point1,point2);

        return distance;
    }

    public static double CalculateDistanceInRadians(Node point1, Node point2)
    {
        double lat1Rad = Math.PI * point1.Latitude / 180;
        double lon1Rad = Math.PI * point1.Longitude / 180;
        double lat2Rad = Math.PI * point2.Latitude / 180;
        double lon2Rad = Math.PI * point2.Longitude / 180;

        double dLat = lat2Rad - lat1Rad;
        double dLon = lon2Rad - lon1Rad;

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return c;
    }
}