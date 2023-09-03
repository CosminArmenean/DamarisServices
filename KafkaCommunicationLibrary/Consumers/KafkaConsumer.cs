using Confluent.Kafka;
using KafkaCommunicationLibrary.Repositories.Interfaces;
using System.Threading.Tasks;
using System;
using static Confluent.Kafka.ConfigPropertyNames;
using static System.Collections.Specialized.BitVector32;

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

        /// <summary>
        /// In this code:        We use Task.Run to execute the blocking _consumer.Consume method asynchronously in a separate task.
        //We use Task.WhenAny to wait for either the Consume task to complete or the cancellation token to be triggered.
        //If the Consume task completes successfully, we retrieve the result and invoke the processMessage action if provided.
        //If the cancellation token is triggered, we throw a TaskCanceledException or return null (you can choose the appropriate handling).
        //This approach ensures that the ConsumeAsync method is truly asynchronous and can be canceled using the CancellationToken.
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="processMessage"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ConsumeResult<TKey, TValue>> ConsumeAsync(string topic, Action<TValue> processMessage, CancellationToken cancellationToken)
        {
            try
            {
                var consumeTask = Task.Run(() => _consumer.Consume(cancellationToken), cancellationToken);

                _logger.LogInformation($"Before Task.WhenAny: IsCancellationRequested = {cancellationToken.IsCancellationRequested}");
                var completedTask = await Task.WhenAny(consumeTask, Task.Delay(-1, cancellationToken));
                _logger.LogInformation($"After Task.WhenAny: IsCancellationRequested = {cancellationToken.IsCancellationRequested}");

                if (completedTask == consumeTask)
                {
                    // The Consume task completed successfully
                    var result = consumeTask.Result;
                    processMessage?.Invoke(result.Message.Value);
                    return result;
                }
                else
                {
                    // The cancellation token was triggered
                    cancellationToken.ThrowIfCancellationRequested();
                    return null; // or throw an appropriate exception
                }
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
