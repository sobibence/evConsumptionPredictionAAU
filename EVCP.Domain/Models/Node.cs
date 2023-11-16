
﻿namespace EVCP.Domain.Models;
﻿using EVCP.Domain.Helpers;


public class Node : BaseEntity
{
    public long NodeIdOsm { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    private List<Edge> _listOfConnectedEdges = new();
    public List<Edge> ListOfConnectedEdges { get { return _listOfConnectedEdges; } }

    private List<Node> _listOfConnectedNodes = new();
    public List<Node> ListOfConnectedNodes { get { return _listOfConnectedNodes; } }


    public override string ToString()
    {
        string connectedNodes = string.Join(", ", _listOfConnectedNodes.Select(node => node.NodeIdOsm));

        return $"Node Info:\n" +
               $"NodeIdOsm: {NodeIdOsm}\n" +
               $"Latitude: {Latitude}\n" +
               $"Longitude: {Longitude}\n" +
               $"Connected Nodes: [{connectedNodes}]";
    }
}
