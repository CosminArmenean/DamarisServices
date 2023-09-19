using App.Metrics;
using Confluent.Kafka;
using Damaris.Kafka.Consumer.Extensions;
using Damaris.Kafka.Consumer.Interfaces;
using Damaris.Kafka.Consumer.Models;
using Damaris.Kafka.Consumer.Telemetry;

namespace Damaris.Kafka.Consumer.Subscribers
{
    public class OrderedKafkaSubscriber : ISubscriber
    {
        private readonly string _topic;
        private readonly ChannelSettings _channelSettings;
        private readonly IMetricsRoot _metrics;
        private readonly ILogger<ISubscriber> _logger;
        private readonly Func<IMessageHandler> _messageHandlerProvider;
        private readonly Func<ConsumerBuilder<string, string>> _kafkaConsumerProvider;

        public OrderedKafkaSubscriber(
            string topic,
            ChannelSettings channelSettings,
            Func<IMessageHandler> messageHandlerProvider,
            IMetricsRoot metrics,
            Func<ConsumerBuilder<string, string>> kafkaConsumerProvider,
            ILogger<ISubscriber> logger)
        {
            _topic = topic;
            _channelSettings = channelSettings;
            _messageHandlerProvider = messageHandlerProvider;
            _metrics = metrics;
            _kafkaConsumerProvider = kafkaConsumerProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            return Task.WhenAll(CreateSubscriptionTasks(stoppingToken));
        }

        private IEnumerable<Task> CreateSubscriptionTasks(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Subscribing to {topic} topic preserving order using {consumers} consumers", _topic, _channelSettings.Consumers);

            // We need a List and not just Enumerable since all threads need to be started before we can await them
            var subscriptions = new List<Task>(_channelSettings.Consumers);
            for (var i = 1; i <= _channelSettings.Consumers; i++)
            {
                subscriptions.Add(Task.Factory.StartNew(async () => await SubscribeAsync(cancellationToken), TaskCreationOptions.LongRunning));
            }

            return subscriptions;
        }

        private async Task SubscribeAsync(CancellationToken stoppingToken)
        {
            IConsumer<string, string> kafkaConsumer = null;

            try
            {
                kafkaConsumer = _kafkaConsumerProvider()
                    .SetPartitionsAssignedHandler((_, partitions) => _logger.LogInformation("Partitions assigned: {}", partitions))
                    .SetPartitionsRevokedHandler((_, partitions) => _logger.LogInformation("Partitions revoked: {}", partitions))
                    .SetPartitionsLostHandler((_, partitions) => _logger.LogInformation("Partitions lost: {}", partitions))
                    .SetLogHandler((_, msg) => _logger.LogKafkaMessage(msg))
                    .Build();

                kafkaConsumer.Subscribe(_topic);

                // Create a new instance of the message handler for each consumer
                // It prevents reentrancy and it's necessary to avoid issues if the client code is not thread safe
                using var messageHandler = _messageHandlerProvider.Invoke();

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        // Code will block here until a message is received or the consumer is closed
                        ConsumeResult<string, string> consumeResult = kafkaConsumer.Consume(stoppingToken);

                        using (_metrics.Measure.Timer.Time(KafkaTelemetry.ProcessingTimer))
                        {
                            // The use of "await" is critical for back pressure, i.e., wait until there's room in the channel to process
                            await messageHandler.ProcessMessageAsync(new Message(consumeResult));
                            kafkaConsumer.StoreOffset(consumeResult);
                        }
                    }
                    catch (ConsumeException e)
                    {
                        _logger.LogError(e.Error.Reason);
                        KafkaTelemetry.ReportError(e);
                    }
                }
            }
            catch (Exception e)
            {
                if (e is not OperationCanceledException)
                    KafkaTelemetry.ReportError(e);

                if (kafkaConsumer != null)
                    this.DisposeKafkaConsumer(kafkaConsumer);
            }
        }

        private void DisposeKafkaConsumer(IConsumer<string, string> consumer)
        {
            try
            {
                _logger.LogInformation("Committing offsets on {topic} topic and exiting the group", _topic);
                consumer.Close();
            }
            finally
            {
                consumer.Dispose();
            }
        }

        public void Dispose()
        {
            // do nothing
        }
    }
}
