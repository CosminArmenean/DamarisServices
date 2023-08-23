namespace KafkaCommunicationLibrary.Domain.Models
{
    public class KafkaSettings
    {
        public string BootstrapServers { get; set; }

        public KafkaConsumerSettings Consumer { get; set; }

        public KafkaProducerSettings Producer { get; set; }
    }
}
