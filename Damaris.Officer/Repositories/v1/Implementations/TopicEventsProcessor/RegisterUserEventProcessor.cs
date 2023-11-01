using Confluent.Kafka;
using Damaris.Domain.v1.Dtos.Requests.Account;
using Damaris.Officer.Repositories.v1.Interfaces.UserInterface;
using KafkaCommunicationLibrary.Repositories.Interfaces;
using Newtonsoft.Json;

namespace Damaris.Officer.Repositories.v1.Implementations.TopicEventsProcessor
{
    public class RegisterUserEventProcessor : IKafkaTopicEventProcessor<string, string>
    {
        private readonly string IDENTITY_REGISTRATION_TOPIC = "user-registration-topic";
        private readonly string IDENTITY_REGISTRATION_RESPONSE_TOPIC = "user-registration-response-topic";
        private readonly string DATA = "REGISTER";

        private readonly ILogger<RegisterUserEventProcessor> _logger;
        private readonly IUserRepository _userRepository;
        string IKafkaTopicEventProcessor<string, string>.Topic => IDENTITY_REGISTRATION_TOPIC;

        string IKafkaTopicEventProcessor<string, string>.ResponseTopic => IDENTITY_REGISTRATION_RESPONSE_TOPIC;
        string IKafkaTopicEventProcessor<string, string>.Data => DATA;

        public RegisterUserEventProcessor(ILogger<RegisterUserEventProcessor> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }
        public async Task<ConsumeResult<string, string>> ProcessEventAsync( string key, string message)
        {
            ConsumeResult<string, string> result = null;
         
                try
                {
                    // Deserialize the JSON message to extract username and password
                    //var loginData = JsonConvert.DeserializeObject<AccountRegistrationRequestDto>(message);

                    // Retrieve the user from the database by username
                    //var user = await _userRepository.GetUserByEmailAsync(loginData.Email);


                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    _logger.LogError($"Error processing login: {ex.Message}");
                }



                _logger.LogInformation($"Received login event: {message}");

                // Return a response if needed
                return await Task.FromResult(result);
           
        }
    }
}
