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

        public ConsumeResult<TKey, TValue> WaitForResponse(string responseTopic, string uniqueKey)
        {
            ConsumeResult<TKey, TValue> response = null;       
            
                _consumer.Subscribe(responseTopic);

                bool responseFound = false;

                while (!responseFound)
                {
                    var consumeResult = _consumer.Consume();
                    var message = consumeResult.Value;

                // Extract unique key from the message
                var parts = new List<string>(); //message.Split(':');
                    var messageKey = parts[0];

                    if (messageKey == uniqueKey)
                    {
                        //response = parts[1]; // Extract the response part from the message
                        responseFound = true;
                    }
                }
            
            return response;
        }
        public void Dispose()
        {
            _consumer.Close();
            _consumer.Dispose();
        }
    }
}
