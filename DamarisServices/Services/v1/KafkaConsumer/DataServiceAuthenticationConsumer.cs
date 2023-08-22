using Damaris.DataService.Repositories.v1.Interfaces.Contracts;
using KafkaCommunicationLibrary.Repositories.Interfaces;
using IKafkaTopicEventProcessor = KafkaCommunicationLibrary.Repositories.Interfaces.IKafkaTopicEventProcessor;

namespace DamarisServices.Services.v1.KafkaConsumer
{
    public class DataServiceAuthenticationConsumer : BackgroundService
    {    
        private readonly IKafkaConsumer _consumer;
        private readonly ILogger<DataServiceAuthenticationConsumer> _logger;

        private readonly IEnumerable<IKafkaTopicEventProcessor> _topicEventProcessors;

        public DataServiceAuthenticationConsumer(ILogger<DataServiceAuthenticationConsumer> logger, IKafkaConsumer consumer, IEnumerable<IKafkaTopicEventProcessor> topicEventProcessors)
        {
            _logger = logger;
            _consumer = consumer;
            // Initialize topic handlers
            _topicEventProcessors = topicEventProcessors;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("DataServiceAuthenticationConsumer is starting.");

            stoppingToken.Register(() =>
                _logger.LogInformation("DataServiceAuthenticationConsumer is stopping."));

            await ConsumeKafkaMessages(stoppingToken);
        }

        private async Task ConsumeKafkaMessages(CancellationToken stoppingToken)
        {
            foreach (var processor in _topicEventProcessors)
            {
                var topic = processor.Topic;

                await _consumer.ConsumeAsync(topic, async message =>
                {
                    try
                    {
                        _logger.LogInformation($"Received Kafka message on topic '{topic}': {message}");

                        // Process the received message using the topic event processor
                        await processor.ProcessEventAsync(message);

                        _logger.LogInformation($"Kafka message on topic '{topic}' processed successfully.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error processing Kafka message on topic '{topic}'.");
                    }
                }, stoppingToken);
            }
        }
    }
}