namespace KafkaCommunicationLibrary.Repositories.Interfaces
{
    public interface IKafkaTopicEventProcessor
    {
        string Topic { get; }
        Task<string> ProcessEventAsync<T>(T value);
        
    }
}
