using EVCP.DataAccess;
using EVCP.Domain.Models;
using EVCP.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EVCP.RouteFinding;

public class DataBaseConnector
{

    private static readonly object Instancelock = new object();
    private static DataBaseConnector instance = new DataBaseConnector();
    public static DataBaseConnector Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DataBaseConnector();
            }
            return instance;
        }
    }

    private DataBaseConnector()
    {
        
    }
}
