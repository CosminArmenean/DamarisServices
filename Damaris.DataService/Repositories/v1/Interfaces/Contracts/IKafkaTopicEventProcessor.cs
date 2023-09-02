namespace Damaris.DataService.Repositories.v1.Interfaces.Contracts
{
    public interface IKafkaTopicEventProcessor<T>
    {
        string Topic { get; }
        Task<string> ProcessEventAsync(T message);
    }
}
