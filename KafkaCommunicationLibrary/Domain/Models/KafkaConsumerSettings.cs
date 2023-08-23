using Confluent.Kafka;

namespace KafkaCommunicationLibrary.Domain.Models
{
    public class KafkaConsumerSettings
    {
        public string GroupId { get; set; }
        public AutoOffsetReset AutoOffsetReset { get; set; }
        public bool EnableAutoCommit { get; set; }
        // Other consumer settings
    }
}
