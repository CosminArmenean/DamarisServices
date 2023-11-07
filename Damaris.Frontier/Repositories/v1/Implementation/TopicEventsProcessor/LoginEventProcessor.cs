using Confluent.Kafka;
using Google.Protobuf.WellKnownTypes;
using KafkaCommunicationLibrary.Repositories.Interfaces;

namespace Damaris.Frontier.Repositories.v1.Implementation.TopicEventsProcessor
{
    public class LoginEventProcessor : IKafkaTopicEventProcessor<string, string>
    {
        private readonly ILogger<LoginEventProcessor> _logger;
        private readonly string IDENTITY_AUTHENTICATION_TOPIC = "user-authentication-topic";
        private readonly string IDENTITY_AUTHENTICATION_RESPONSE_TOPIC = "user-authentication-response-topic";
        private readonly string DATA = "LOGIN";
        public string Topic => IDENTITY_AUTHENTICATION_TOPIC;
        public string ResponseTopic => IDENTITY_AUTHENTICATION_RESPONSE_TOPIC;
        public string Data => DATA;

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
        public async Task<ConsumeResult<string, string>> ProcessEventAsync( string key, string message)
        {
            // Handle login event logic
            //await Task.Delay(TimeSpan.FromSeconds(2)); // Simulate processing time
            ConsumeResult<string, string> result = null;
            return await  Task.FromResult(result);
        }
    }
}
