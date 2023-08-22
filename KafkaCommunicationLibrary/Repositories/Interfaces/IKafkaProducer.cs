using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace KafkaCommunicationLibrary.Repositories.Interfaces
{
    public interface IKafkaProducer<TKey, TValue>
    {
        Task PublishAsync(string topic, TKey key, TValue value);
    }
}
