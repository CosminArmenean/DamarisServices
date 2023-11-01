using Confluent.Kafka;
using Damaris.Common.v1.CommonUtilities.JsonUtilities;
using Damaris.Common.v1.Constants;
using Damaris.DataService.Repositories.v1.Implementation.TopicEventsProcessor;
using DnsClient;
using KafkaCommunicationLibrary.Consumers;
using KafkaCommunicationLibrary.Producers;
using KafkaCommunicationLibrary.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Org.BouncyCastle.Crypto.Tls;
using StackExchange.Redis;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Damaris.DataService.Services.v1.KafkaConsumer
{
    public class IdentityServiceConsumer : BackgroundService, IKafkaTopicProcessor<ConsumeResult<string, string>>
    {
        private readonly string DATA = "REGISTER";
        private readonly string IDENTITY_AUTHENTICATION_TOPIC = "user-authentication-topic";
        private readonly KafkaConsumer<string, string> _consumer;
        private readonly KafkaProducer<string, string> _producer;
        private readonly ILogger<IdentityServiceConsumer> _logger;
        public string Topic => IDENTITY_AUTHENTICATION_TOPIC;

        private readonly IEnumerable<IKafkaTopicEventProcessor<string, string>> _topicEventProcessors;

        public IdentityServiceConsumer(ILogger<IdentityServiceConsumer> logger, KafkaProducer<string, string> producer, KafkaConsumer<string, string> consumer, IEnumerable<IKafkaTopicEventProcessor<string, string>> topicProcessors)
        {
            _logger = logger;
            _consumer = consumer;
            _producer = producer;
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
                            ConsumeResult<string, string>? consumeResult = await _consumer.ConsumeAsync(stoppingToken);
                            if(consumeResult != null)
                            {
                                var jsonLoginEvent = consumeResult != null ? consumeResult.Message.Value : "";
                                _logger.LogInformation($"Received Kafka message on topic '{topic}': {topic}");
                                // Process the received message using the topic event processor
                                //string eventType = ExtractValueFromJson.ExtractAttributeValue(jsonLoginEvent, JsonAttributes.RequestTypeAttribute);

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
