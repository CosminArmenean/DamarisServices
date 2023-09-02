using Confluent.Kafka;
using Google.Protobuf.WellKnownTypes;
using KafkaCommunicationLibrary.Repositories.Interfaces;

namespace DamarisServices.Repositories.v1.Implementation.TopicEventsProcessor
{
    public class LoginEventProcessor : IKafkaTopicEventProcessor<string>
    {
        private readonly string IDENTITY_AUTHENTICATION_TOPIC = "user-authentication-topic";
        public string Topic => IDENTITY_AUTHENTICATION_TOPIC;

        //public async Task<string> ProcessEventAsync(string message)
        //{
        //    // Handle login event logic
        //    await Task.Delay(TimeSpan.FromSeconds(2)); // Simulate processing time
        //    return message;

        //}

        public async Task<string> ProcessEventAsync<T>(string topic, T value)
        {
            // Handle login event logic
            //await Task.Delay(TimeSpan.FromSeconds(2)); // Simulate processing time
            string result = $"Processed: {value}";
            return await Task.FromResult(result);
        }
    }
}
