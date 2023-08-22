namespace Damaris.DataService.Repositories.v1.Interfaces.Contracts
{
    public interface IKafkaTopicEventProcessor
    {
        string Topic { get; }
        Task ProcessEventAsync(string message);
    }
}
