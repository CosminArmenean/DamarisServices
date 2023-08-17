using Confluent.Kafka;

namespace KafkaCommunicationLibrary.Consumers
{
    public class KafkaConsumer<TKey, TValue> : IDisposable
    {
        private readonly IConsumer<TKey, TValue> _consumer;
        private readonly ILogger<KafkaConsumer<TKey, TValue>> _logger;

        public KafkaConsumer(ConsumerConfig config, ILogger<KafkaConsumer<TKey, TValue>> logger)
        {
            _consumer = new ConsumerBuilder<TKey, TValue>(config).Build();
            _logger = logger;
        }

        public void Subscribe(string topic)
        {
            _consumer.Subscribe(topic);
        }

        public ConsumeResult<TKey, TValue> Consume()
        {
            try
            {
                return _consumer.Consume();
            }
            catch (ConsumeException ex)
            {
                _logger.LogError($"Consume error: {ex.Error.Reason}");
                return null;
            }
        }
        public void Dispose()
        {
            _consumer.Close();
            _consumer.Dispose();
        }
    }
}
