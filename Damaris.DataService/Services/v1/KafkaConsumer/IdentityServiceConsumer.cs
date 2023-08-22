using Damaris.DataService.Repositories.v1.Interfaces.Contracts;
using KafkaCommunicationLibrary.Repositories.Interfaces;

namespace Damaris.DataService.Services.v1.KafkaConsumer
{
    public class IdentityServiceConsumer : BackgroundService
    {        
        private readonly IKafkaConsumer _consumer;
        private readonly ILogger<IdentityServiceConsumer> _logger;       

        private readonly IEnumerable<KafkaCommunicationLibrary.Repositories.Interfaces.IKafkaTopicEventProcessor> _topicEventProcessors;

        public IdentityServiceConsumer(ILogger<IdentityServiceConsumer> logger, IKafkaConsumer consumer, IEnumerable<KafkaCommunicationLibrary.Repositories.Interfaces.IKafkaTopicEventProcessor> topicEventProcessors)
        {
            _logger = logger;
            _consumer = consumer;
            // Initialize topic handlers
            _topicEventProcessors = topicEventProcessors;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("IdentityServiceConsumer is starting.");

            stoppingToken.Register(() =>
                _logger.LogInformation("IdentityServiceConsumer is stopping."));

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
