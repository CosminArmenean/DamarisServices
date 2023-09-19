using App.Metrics;
using Confluent.Kafka;
using Damaris.Kafka.Consumer.Extensions;
using Damaris.Kafka.Consumer.Factories;
using Damaris.Kafka.Consumer.Interfaces;
using Damaris.Kafka.Consumer.Models;
using Damaris.Kafka.Consumer.Telemetry;
using System.Threading.Channels;

namespace Damaris.Kafka.Consumer.Subscribers
{
    /// <summary>
    /// Implements an Apache Kafka subscriber 
    /// </summary>
    public sealed class UnorderedKafkaSubscriber : ISubscriber
    {
        private readonly string _topic;
        private readonly ChannelSettings _channelSettings;
        private readonly ILogger<ISubscriber> _logger;
        private readonly Channel<Message> _channel;
        private readonly Func<IMessageHandler> _messageHandlerProvider;
        private readonly IMetricsRoot _metrics;
        private readonly Func<ConsumerBuilder<string, string>> _kafkaConsumerProvider;
        private IConsumer<string, string> _kafkaConsumer;

        /// <summary>
        /// Creates an Apache Kafka subscriber 
        /// </summary>
        public UnorderedKafkaSubscriber(
            string topic,
            ChannelSettings channelSettings,
            Func<IMessageHandler> messageHandlerProvider,
            IMetricsRoot metrics,
            Func<ConsumerBuilder<string, string>> kafkaConsumerProvider,
            ILogger<ISubscriber> logger)
        {
            _topic = topic;
            _channelSettings = channelSettings;
            _logger = logger;
            _messageHandlerProvider = messageHandlerProvider;
            _metrics = metrics;
            _kafkaConsumerProvider = kafkaConsumerProvider;
            _channel = ChannelFactory.CreateChannel(channelSettings);
        }

        /// <inheritdoc />
        public async Task StartAsync(CancellationToken stoppingToken)
        {
            var consumptionTasks = CreateConsumptionTasks(stoppingToken);
            // We need to run this in a separate thread to avoid blocking the main thread
            await Task.Factory.StartNew(async () => await SubscribeAsync(stoppingToken), TaskCreationOptions.LongRunning);
            await Task.WhenAll(consumptionTasks);
        }

        private async Task SubscribeAsync(CancellationToken stoppingToken)
        {
            try
            {
                _kafkaConsumer = _kafkaConsumerProvider()
                    .SetPartitionsAssignedHandler((_, partitions) => _logger.LogInformation("Partitions assigned: {}", partitions))
                    .SetPartitionsRevokedHandler((_, partitions) => _logger.LogInformation("Partitions revoked: {}", partitions))
                    .SetPartitionsLostHandler((_, partitions) => _logger.LogInformation("Partitions lost: {}", partitions))
                    .SetLogHandler((_, msg) => _logger.LogKafkaMessage(msg))
                    .Build();

                _kafkaConsumer.Subscribe(_topic);

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        // Code will block here until a message is received or the consumer is closed
                        var consumeResult = _kafkaConsumer.Consume(stoppingToken);
                        // The use of "await" is critical for back pressure, i.e., wait until there's room in the channel to process
                        await _channel.Writer.WriteAsync(new Message(consumeResult), stoppingToken);
                    }
                    catch (ConsumeException e)
                    {
                        _logger.LogError(e.Error.Reason);
                        KafkaTelemetry.ReportError(e);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Committing offsets and exiting the group");
                _kafkaConsumer.Close();
            }
            catch (Exception e)
            {
                KafkaTelemetry.ReportError(e);
                _channel.Writer.TryComplete();
                this.Dispose();
            }
        }

        private IEnumerable<Task> CreateConsumptionTasks(CancellationToken cancellationToken = default)
        {
            var preserving = _channelSettings.Consumers == 1 ? "preserving" : "not preserving";
            _logger.LogInformation("Subscribing to '{topic}' topic {preserving} order using {consumers} consumers", _topic, preserving, _channelSettings.Consumers);

            // We need a List and not just Enumerable since all threads need to be started before we can await them
            var tasks = new List<Task>();
            for (var i = 1; i <= _channelSettings.Consumers; i++)
            {
                tasks.Add(Task.Factory.StartNew(async () => await ConsumeAsync(cancellationToken), TaskCreationOptions.LongRunning));
            }

            return tasks;
        }

        private async Task ConsumeAsync(CancellationToken cancellationToken)
        {
            try
            {
                // Create a new instance of the message handler for each consumer
                // It prevents reentrancy and it's necessary to avoid issues if the client code is not thread safe
                using var messageHandler = _messageHandlerProvider.Invoke();

                await foreach (Message? message in _channel.Reader.ReadAllAsync(cancellationToken))
                {
                    using (_metrics.Measure.Timer.Time(KafkaTelemetry.ProcessingTimer))
                    {
                        try
                        {
                            await messageHandler.ProcessMessageAsync(message);
                            _kafkaConsumer.StoreOffset(message.Raw);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex,
                                "Unhandled processing exception on IMessageHandler for message ({TopicPartitionOffset})",
                                message.Raw.TopicPartitionOffset);
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            }
        }

        /// <summary>
        /// Commit pending offsets, exit groups, and dispose resources
        /// </summary>
        public void Dispose()
        {
            try
            {
                _logger.LogInformation("Committing offsets on {topic} topic and exiting the group", _topic);
                _kafkaConsumer?.Close();
            }
            finally
            {
                _kafkaConsumer?.Dispose();
            }
        }
    }
}
