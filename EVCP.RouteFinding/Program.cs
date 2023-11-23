// See https://aka.ms/new-console-template for more information
using EVCP.DataAccess;
using EVCP.Domain.Models;
using EVCP.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

class Program
{
    public static async Task Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        
        builder.Services.AddSingleton<DapperContext>();
        builder.Logging.AddConsole();
        builder.Services.AddTransient<IEdgeRepository, EdgeRepository>();
        builder.Services.AddTransient<IFEstConsumptionRepository, FEstConsumptionRepository>();
        builder.Services.AddTransient<IFRecordedTravelRepository, FRecordedTravelRepository>();
        builder.Services.AddTransient<INodeRepository, NodeRepository>();
        builder.Services.AddTransient<IProducerRepository, ProducerRepository>();
        builder.Services.AddTransient<IVehicleModelRepository, VehicleModelRepository>();
        builder.Services.AddTransient<IVehicleTripStatusRepository, VehicleTripStatusRepository>();
        builder.Services.AddTransient<IWeatherRepository, WeatherRepository>();
        
    
        IHost host = builder.Build();
        
        
        Node node = new Node();
        List<Node> listOfNodes = new() { node };
        host.Services.GetService<INodeRepository>().Create(listOfNodes);
        
        var asd = await host.Services.GetService<INodeRepository>().GetByIdAsync(0);
        
        
        
        
        
        
        
        host.Run();
        
    }


}