using Confluent.Kafka;

namespace KafkaCommunicationLibrary.Repositories.Interfaces
{
    public interface IKafkaTopicEventProcessor<TKey, TValue>
    {
        string Topic { get; }
        string ResponseTopic { get; }
        Task<ConsumeResult<TKey, TValue>> ProcessEventAsync(string eventType, TKey key, TValue value);
        
    }
}
