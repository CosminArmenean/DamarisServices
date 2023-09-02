using Confluent.Kafka;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace KafkaCommunicationLibrary.Repositories.Interfaces
{
    public interface IKafkaConsumer<TKey, TValue>
    {
        ConsumeResult<TKey, TValue> Consume();
        Task<ConsumeResult<TKey, TValue>> WaitForResponse(string responseTopic, TKey key, TValue value);
    }
}
