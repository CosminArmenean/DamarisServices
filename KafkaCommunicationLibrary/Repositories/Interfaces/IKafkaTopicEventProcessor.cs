using Confluent.Kafka;

namespace KafkaCommunicationLibrary.Repositories.Interfaces
{
    public interface IKafkaTopicEventProcessor<TKey, TValue>
    {
        string Data { get; }
        string Topic { get; }
        string ResponseTopic { get; }
        Task<ConsumeResult<TKey, TValue>> ProcessEventAsync(TKey key, TValue value);
        
    }
}
