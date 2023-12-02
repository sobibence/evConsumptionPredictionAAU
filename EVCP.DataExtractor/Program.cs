using EasyNetQ;
using EVCP.DataExtractor;
using EVCP.DataPublisher;
using EVCP.Dtos;

var bus = Bootstrapper.RegisterBus();
var messagePublisher = new EVDataPublisher(bus);

var worker = new Worker(messagePublisher);

worker.Run().Wait();

// test consumer
//bus.PubSub.Subscribe<IEVData>("test", HandleMessage);
bus.PubSub.Subscribe<ITripDataDto>("test", HandleMessage2);
Console.WriteLine("Listening for messages. Hit <return> to quit.");
Console.ReadLine();

void HandleMessage(IEVData message)
{
    Console.WriteLine(message.Name);
}

void HandleMessage2(ITripDataDto message)
{
    Console.WriteLine(message.Data.FirstOrDefault().TripId);
}