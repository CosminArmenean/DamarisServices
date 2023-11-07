using Confluent.Kafka;
using Damaris.Common.v1.CommonUtilities.JsonUtilities;
using Damaris.Common.v1.Constants;
using KafkaCommunicationLibrary.Consumers;
using KafkaCommunicationLibrary.Repositories.Interfaces;

namespace Damaris.Frontier.Services.v1.KafkaConsumer
{
    public class DataServiceAuthenticationConsumer : BackgroundService
    {    
        private readonly KafkaConsumer<string, string> _consumer;
        private readonly ILogger<DataServiceAuthenticationConsumer> _logger;

        private readonly IEnumerable<IKafkaTopicProcessor<string>> _topicEventProcessors;

        public DataServiceAuthenticationConsumer(ILogger<DataServiceAuthenticationConsumer> logger, KafkaConsumer<string, string> consumer, IEnumerable<IKafkaTopicProcessor<string>> topicProcessors)
        {
            _logger = logger;
            _consumer = consumer;
            // Initialize topic handlers
            _topicEventProcessors = topicProcessors;
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
            Parallel.ForEach(_topicEventProcessors, async processor =>
            {
                var topic = processor.Topic;
                try
                {

                    _consumer.Subscribe(topic);
                    ConsumeResult<string, string>? consumeResult = _consumer.Consume();
                    var jsonLoginEvent = consumeResult.Message.Value;
                    _logger.LogInformation($"Received Kafka message on topic '{topic}': {topic}");
                    // Process the received message using the topic event processor
                    string eventType = ExtractValueFromJson.ExtractAttributeValue(jsonLoginEvent, JsonAttributes.RequestTypeAttribute);

                    var response = await processor.ProcessEventAsync(consumeResult.Message.Key, jsonLoginEvent, eventType);


                }
                catch (Exception ex)
                {

                    _logger.LogError(ex, $"Error processing Kafka message on topic '{topic}'.");
                }                
            });
        }

        public async Task<ConsumeResult<string, string>> ProcessEventAsync(string key, string message, string eventType)
        {
            ConsumeResult<string, string> response = null;
            try
            {
                if (eventType == EventTypes.LOGIN)
                {
                    //LoginEventProcessor loginEventProcessor = new LoginEventProcessor() { }
                }
                else if (eventType == EventTypes.LOGOUT)
                {

                }
                else
                {

                }
                return await Task.FromResult(response); ;
            }
            catch (Exception ex)
            {
                return response;
                _logger.LogInformation($"IdentityServiceConsumer logged error on Process Event: {ex}");
            }
        }
    }
}