using EVCP.DataConsumer;
using EVCP.DataConsumer.Consumer;
using EVCP.DataConsumer.Publisher;
using EVCP.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

var bus = Bootstrapper.RegisterBus();
var serviceProvider = Bootstrapper.RegisterServices();

var publisher = new TripDataPublisher(bus);
var publishWorker = new PublishWorker(publisher, "1.A", new MessageGenerator(3, 5).GenerateTripMessage);

var handler = new TripDataHandler(
    serviceProvider.GetService<IEdgeRepository>(),
    serviceProvider.GetService<IFEstConsumptionRepository>(),
    serviceProvider.GetService<IFRecordedTravelRepository>(),
    serviceProvider.GetService<IWeatherRepository>());
var consumer = new TripDataConsumer(bus, "test", "1.*");
var consumerWorker = new ConsumeWorker(consumer, handler, "consumer");

publishWorker.Run().Wait();

Thread.Sleep(2000);

consumerWorker.Run().Wait();

Console.WriteLine("Listening for messages. Hit <return> to quit.");
Console.ReadLine();