namespace EVCP.DataExtractor
{
    public class Worker : IWorker
    {
        private readonly IMessagePublisher _messagePublisher;

        public Worker(IMessagePublisher messagePublisher)
        {
            _messagePublisher = messagePublisher ?? throw new ArgumentNullException(nameof(messagePublisher));
        }

        public async Task Run()
        {
            await ProcessFile();
        }

        private async Task ProcessFile()
        {
            var data = new EVData[] { new EVData("testing publish") };
            var publishTasks = data.Select(_messagePublisher.Publish);
            await Task.WhenAll(publishTasks);
        }
    }

    public interface IWorker
    {
        public Task Run();
    }
}