using Google.Protobuf.WellKnownTypes;
using KafkaCommunicationLibrary.Repositories.Interfaces;

namespace Damaris.DataService.Repositories.v1.Implementation.TopicEventsProcessor
{
    public class LoginEventProcessor : IKafkaTopicEventProcessor<string>
    {
        private readonly ILogger<LoginEventProcessor> _logger;
        private readonly string IDENTITY_AUTHENTICATION_TOPIC = "user-authentication-topic";
        public string Topic => IDENTITY_AUTHENTICATION_TOPIC;

        public LoginEventProcessor(ILogger<LoginEventProcessor> logger)
        {
            _logger = logger;
        }
        public async Task<string> ProcessEventAsync<T>(string topic, T message)
        {
            if (message is string loginEvent)
            {
                // You can implement your login event processing logic here.
                // For example, you might want to parse the login event and perform some actions.
                // Here, we'll simply log the event.

                _logger.LogInformation($"Received login event: {loginEvent}");

                // Return a response if needed
                return "Login event processed successfully.";
            }
            else
            {
                _logger.LogWarning($"Received an invalid login event of type {typeof(T)}");
                return "Invalid login event.";
            }

        }
    }
}
