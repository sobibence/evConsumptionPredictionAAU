using EVCP.DataConsumer;
using EVCP.DataConsumer.Consumer;
using EVCP.DataConsumer.Publisher;

var bus = Bootstrapper.RegisterBus();
var publisher = new EVDataPublisher(bus);
var consumer = new EVDataConsumer(bus);

var publishWorker = new PublishWorker(publisher);
var consumeWorker = new ConsumeWorker(consumer);

PublishWorker.NoOfMessages = 5;
PublishWorker.NoOfElementsInMessage = 5;

publishWorker.Run().Wait();
consumeWorker.Run().Wait();

//bus.PubSub.Subscribe<IEVDataDto<IEVItemDto>>("test", msgs => msgs.Data.ToList().ForEach(msg => Console.WriteLine(msg.Name)));

Console.WriteLine("Listening for messages. Hit <return> to quit.");
Console.ReadLine();