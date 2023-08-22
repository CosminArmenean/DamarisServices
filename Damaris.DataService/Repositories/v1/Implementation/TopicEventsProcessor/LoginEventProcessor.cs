using Damaris.DataService.Repositories.v1.Interfaces.Contracts;

namespace Damaris.DataService.Repositories.v1.Implementation.TopicEventsProcessor
{
    public class LoginEventProcessor : IKafkaTopicEventProcessor
    {
        private readonly string IDENTITY_AUTHENTICATION_TOPIC = "user-authentication-topic";
        public string Topic => IDENTITY_AUTHENTICATION_TOPIC;

        public async Task ProcessEventAsync(string message)
        {
            // Handle login event logic
            await Task.Delay(TimeSpan.FromSeconds(2)); // Simulate processing time

        }
    }
}
