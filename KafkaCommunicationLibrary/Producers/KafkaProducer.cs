using Confluent.Kafka;
using System.Collections.Concurrent;
using System.Diagnostics.Metrics;
using System.Numerics;
using System.Security.Policy;

namespace KafkaCommunicationLibrary.Producers
{
    public class KafkaProducer<TKey, TValue> : IDisposable
    {
        private readonly IProducer<TKey, TValue> _producer;
        private readonly ILogger<KafkaProducer<TKey, TValue>> _logger;

        public KafkaProducer(ProducerConfig config, ILogger<KafkaProducer<TKey, TValue>> logger)
        {
            _producer = new ProducerBuilder<TKey, TValue>(config).Build();
            _logger = logger;
        }

        /// <summary>
        /// In the above code, the deliveryReport variable holds the asynchronous task returned by _producer.ProduceAsync. The ContinueWith method is used to attach a continuation to the task, which is executed when the task completes (either successfully or with an error). In this case, we're using it to log any errors that might occur during the asynchronous message production process.

        //Please note that the ContinueWith method has been replaced by more modern asynchronous patterns in newer versions of C# (such as await and async), but I used it here to show how to handle asynchronous task continuations in the context of the provided example.
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Produce(string topic, TKey key, TValue value)
        {
            try
            {
                var deliveryReport = _producer.ProduceAsync(topic, new Message<TKey, TValue> { Key = key, Value = value });

                deliveryReport.ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        _logger.LogError($"Error producing message: {task.Exception}");
                    }
                });
            }
            catch(ProduceException<TKey, TValue> ex) 
            {
                _logger.LogError($"Produce error: {ex.Error.Reason}");
            }
        }
        public void Dispose()
        {
            _producer.Dispose();
        }
    }
}
