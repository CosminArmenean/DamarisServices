namespace KafkaCommunicationLibrary.Repositories.Interfaces
{
    public interface IKafkaTopicEventProcessor<T>
    {
        string Topic { get; }
        Task<string> ProcessEventAsync<T>(string topic, T value);
        
    }
}
