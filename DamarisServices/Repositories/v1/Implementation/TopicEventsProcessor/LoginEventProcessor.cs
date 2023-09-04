using Confluent.Kafka;
using Google.Protobuf.WellKnownTypes;
using KafkaCommunicationLibrary.Repositories.Interfaces;

namespace DamarisServices.Repositories.v1.Implementation.TopicEventsProcessor
{
    public class LoginEventProcessor : IKafkaTopicEventProcessor<string, string>
    {
        private readonly ILogger<LoginEventProcessor> _logger;
        private readonly string IDENTITY_AUTHENTICATION_TOPIC = "user-authentication-topic";
        public string Topic => IDENTITY_AUTHENTICATION_TOPIC;

        //public async Task<string> ProcessEventAsync(string message)
        //{
        //    // Handle login event logic
        //    await Task.Delay(TimeSpan.FromSeconds(2)); // Simulate processing time
        //    return message;

        //}

        public LoginEventProcessor(ILogger<LoginEventProcessor> logger)
        {
            _logger = logger;  
        }
        public async Task<ConsumeResult<string, string>> ProcessEventAsync(string eventType, string key, string message)
        {
            // Handle login event logic
            //await Task.Delay(TimeSpan.FromSeconds(2)); // Simulate processing time
            ConsumeResult<string, string> result = null;
            return await  Task.FromResult(result);
        }
    }
}
