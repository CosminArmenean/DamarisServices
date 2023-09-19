using Confluent.Kafka;
using Damaris.Common.v1.CommonUtilities.JsonUtilities;
using Damaris.Common.v1.Constants;
using KafkaCommunicationLibrary.Consumers;
using KafkaCommunicationLibrary.Repositories.Interfaces;
using Org.BouncyCastle.Crypto.Tls;

namespace Damaris.Officer.Services.v1
{
    public class OfficerConsumerService : BackgroundService, IKafkaTopicProcessor<ConsumeResult<string, string>>
    {
        private readonly string IDENTITY_AUTHENTICATION_TOPIC = "user-authentication-topic";
        private readonly KafkaConsumer<string, string> _consumer;
        private readonly ILogger<OfficerConsumerService> _logger;
        public string Topic => IDENTITY_AUTHENTICATION_TOPIC;

        private readonly IEnumerable<IKafkaTopicEventProcessor<string, string>> _topicEventProcessors;

        public OfficerConsumerService(ILogger<OfficerConsumerService> logger, KafkaConsumer<string, string> consumer, IEnumerable<IKafkaTopicEventProcessor<string, string>> topicProcessors)
        {
            _logger = logger;
            _consumer = consumer;
            // Initialize topic handlers
            _topicEventProcessors = topicProcessors;
        }



        public async Task<ConsumeResult<string, string>> ProcessEventAsync(string eventType, string key, string message)
        {
            ConsumeResult<string, string> response = null;
            try
            {
                if (eventType == EventTypes.LOGIN)
                {

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
                _consumer.Subscribe(topic);
                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        try
                        {
                           
                            ConsumeResult<string, string>? consumeResult = await _consumer.ConsumeAsync(stoppingToken);
                            if(consumeResult != null)
                            {
                                var jsonLoginEvent = consumeResult.Message.Value;
                                _logger.LogInformation($"Received Kafka message on topic '{topic}': {topic}");
                                // Process the received message using the topic event processor
                                string eventType = ExtractValueFromJson.ExtractAttributeValue(jsonLoginEvent, JsonAttributes.RequestTypeAttribute);

                                var response = await processor.ProcessEventAsync(eventType, consumeResult.Message.Key, jsonLoginEvent);

                            }
                            else
                            {
                                
                            }

                        }
                        catch (OperationCanceledException)
                        {
                            // Handle cancellation if needed
                            _logger.LogInformation("Kafka message consumption canceled.");
                            break;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Error processing Kafka message on topic '{topic}'.");
                        }
                    }
                }
                catch (Exception ex)
                {

                    _logger.LogError(ex, $"Error processing Kafka message on topic '{topic}'.");
                }
            }
        }

        public Task ConsumeAsync(string topic, Action<string> processMessage, CancellationToken cancellationToken)
        {
            return null;
        }

        public Task<string> WaitForResponse(string responseTopic, string key)
        {
            return Task.FromResult("I'm here");
        }
    }
}
