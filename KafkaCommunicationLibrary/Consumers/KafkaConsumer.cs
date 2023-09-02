using Confluent.Kafka;
using KafkaCommunicationLibrary.Repositories.Interfaces;

namespace KafkaCommunicationLibrary.Consumers
{
    public class KafkaConsumer<TKey, TValue> : IDisposable, IKafkaConsumer<TKey, TValue>
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
                ConsumeResult<TKey, TValue> consumeResult = _consumer.Consume();
                if(consumeResult != null && consumeResult.Message.Key != null && consumeResult.Message.Key.ToString() == uniqueKey)
                {
                    var message = consumeResult.Value;
                    //do stuff
                    responseFound = true;
                }  
            }            
            return response;
        }
               

        public Task<ConsumeResult<TKey, TValue>> ConsumeAsync(string topic, Action<string> processMessage, CancellationToken cancellationToken)
        {
            try
            {
                var result =  _consumer.Consume();
                return null;
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

       

        public Task<ConsumeResult<TKey, TValue>> WaitForResponse(string responseTopic, TKey key, TValue value)
        {
            throw new NotImplementedException();
        }
    }
}
