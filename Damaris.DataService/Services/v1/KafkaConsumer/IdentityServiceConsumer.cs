using Confluent.Kafka;
using Damaris.DataService.Repositories.v1.Interfaces.Contracts;
using KafkaCommunicationLibrary.Consumers;
using KafkaCommunicationLibrary.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Org.BouncyCastle.Crypto.Tls;
using StackExchange.Redis;
using IKafkaTopicEventProcessor = KafkaCommunicationLibrary.Repositories.Interfaces.IKafkaTopicEventProcessor<string>;

namespace Damaris.DataService.Services.v1.KafkaConsumer
{
    public class IdentityServiceConsumer : BackgroundService, IKafkaTopicEventProcessor
    {
        private readonly string IDENTITY_AUTHENTICATION_TOPIC = "user-authentication-topic";
        private readonly KafkaConsumer<string, string> _consumer;
        private readonly ILogger<IdentityServiceConsumer> _logger;
        public string Topic => IDENTITY_AUTHENTICATION_TOPIC;

        private readonly IEnumerable<IKafkaTopicEventProcessor> _topicEventProcessors;

        public IdentityServiceConsumer(ILogger<IdentityServiceConsumer> logger, KafkaConsumer<string, string> consumer, IEnumerable<IKafkaTopicEventProcessor> topicEventProcessors)
        {
            _logger = logger;
            _consumer = consumer;
            // Initialize topic handlers
            _topicEventProcessors = topicEventProcessors;
        }

       

        public Task<string> ProcessEventAsync<T>(string topic, T value)
        {
            return Task.FromResult( "I'm here");
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
                    //if(jsonLoginEvent)
                    string response = await processor.ProcessEventAsync(topic, consumeResult);
                }
                catch (Exception ex)
                {

                    _logger.LogError(ex, $"Error processing Kafka message on topic '{topic}'.");
                }
                
                //await _consumer.ConsumeAsync(topic, async message =>
                //{
                //    try
                //    {
                //        _logger.LogInformation($"Received Kafka message on topic '{topic}': {message}");

                //        // Process the received message using the topic event processor
                //        string response = await processor.ProcessEventAsync(topic, message);

                //        _logger.LogInformation($"Kafka message on topic '{topic}' processed successfully.");
                //    }
                //    catch (Exception ex)
                //    {
                //        _logger.LogError(ex, $"Error processing Kafka message on topic '{topic}'.");
                //    }
                //}, stoppingToken);
            });
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
