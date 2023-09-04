using Confluent.Kafka;

namespace KafkaCommunicationLibrary.Repositories.Interfaces
{
    public interface IKafkaTopicProcessor<T>
    {
        string Topic { get; }
        Task<ConsumeResult<string, string>> ProcessEventAsync(string key, string message, string eventType);
    }
}
