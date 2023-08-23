using Confluent.Kafka;

namespace KafkaCommunicationLibrary.Domain.Models
{
    public class KafkaProducerSettings
    {
        public Acks? Acks { get; set; }
        public int RetriesBackoffMs { get; set; }
        public int MaxInFlight { get; set; }
        // Other producer settings
    }
   






}
