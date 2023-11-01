using Confluent.Kafka;
using Damaris.Common.v1.CommonUtilities.JsonUtilities;
using Damaris.Common.v1.Constants;
using KafkaCommunicationLibrary.Consumers;
using KafkaCommunicationLibrary.Producers;
using KafkaCommunicationLibrary.Repositories.Interfaces;
using System.Threading;

namespace Damaris.DataService.Services.v1.KafkaConsumer
{
    public class AuthenticationServiceConsumer : BackgroundService, IKafkaTopicProcessor<ConsumeResult<string, string>>
    {
        private readonly string DATA = "LOGIN";
        private readonly string IDENTITY_AUTHENTICATION_TOPIC = "user-authentication-topic";
        private readonly KafkaConsumer<string, string> _consumer;
        private readonly KafkaProducer<string, string> _producer;
        private readonly ILogger<AuthenticationServiceConsumer> _logger;
        public string Topic => IDENTITY_AUTHENTICATION_TOPIC;

        private readonly IEnumerable<IKafkaTopicEventProcessor<string, string>> _topicEventProcessors;
        public AuthenticationServiceConsumer(ILogger<AuthenticationServiceConsumer> logger, KafkaProducer<string, string> producer, KafkaConsumer<string, string> consumer, IEnumerable<IKafkaTopicEventProcessor<string, string>> topicProcessors)
        {
            _logger = logger;
            _consumer = consumer;
            _producer = producer;
            // Initialize topic handlers
            _topicEventProcessors = topicProcessors;

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("AuthenticationServiceConsumer is starting.");

            stoppingToken.Register(() =>
                _logger.LogInformation("AuthenticationServiceConsumer is stopping."));

            await ConsumeKafkaMessages(stoppingToken);
        }


        private async Task ConsumeKafkaMessages(CancellationToken stoppingToken)
        {
          
            IKafkaTopicEventProcessor<string, string> processor = _topicEventProcessors.Where(p => p.Data == DATA).First();
            
            var topic = processor.Topic;
            var responsTopic = processor.ResponseTopic;
            _consumer.Subscribe(topic);
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var timeout = TimeSpan.FromSeconds(1);
                        ConsumeResult<string, string>? consumeResult = await _consumer.ConsumeAsync(stoppingToken);
                        if (consumeResult != null)
                        {
                            var jsonLoginEvent = consumeResult != null ? consumeResult.Message.Value : "";
                            _logger.LogInformation($"Received Kafka message on topic '{topic}': {topic}");
                            // Process the received message using the topic event processor
                            string eventType = ExtractValueFromJson.ExtractAttributeValue(jsonLoginEvent, JsonAttributes.RequestTypeAttribute);

                            var response = await processor.ProcessEventAsync(consumeResult.Message.Key, jsonLoginEvent);

                            bool messageProduced = await _producer.Produce(responsTopic, consumeResult.Message.Key, response.Value);
                            if (messageProduced)
                            {
                                _logger.LogInformation($"Produced Kafka message on topic '{responsTopic}':Message: {response}");
                            }
                            else
                            {
                                _logger.LogError($"Failed to produce Kafka message on topic '{responsTopic}':Message: {response} : Key: {consumeResult.Message.Key}");

                            }
                        }
                        else
                        {
                            // No more messages to consume, you can break the loop to stop the consumer.
                            return;
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


        public Task<ConsumeResult<string, string>> ProcessEventAsync(string key, string message, string eventType)
        {
            return null;
        }
    }
}
