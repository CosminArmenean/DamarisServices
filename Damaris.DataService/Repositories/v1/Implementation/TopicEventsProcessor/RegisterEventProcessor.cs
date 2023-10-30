using Confluent.Kafka;
using Damaris.DataService.Repositories.v1.Implementation.UserImplementation;
using Damaris.DataService.Repositories.v1.Interfaces.UserInterfaces;
using Damaris.Domain.v1.Dtos.Requests.Account;
using Damaris.Domain.v1.Models.User;
using KafkaCommunicationLibrary.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Damaris.DataService.Repositories.v1.Implementation.TopicEventsProcessor
{
    public class RegisterEventProcessor : IKafkaTopicEventProcessor<string, string>
    {
        private readonly ILogger<LoginEventProcessor> _logger;
        private readonly IUserRepository _userRepository;
        private readonly string IDENTITY_REGISTRATION_TOPIC = "user-registration-topic";
        private readonly string IDENTITY_REGISTRATION_RESPONSE_TOPIC = "user-registration-response-topic";
        public string Topic => IDENTITY_REGISTRATION_TOPIC;
        public string ResponseTopic => IDENTITY_REGISTRATION_RESPONSE_TOPIC;
        public RegisterEventProcessor(ILogger<LoginEventProcessor> logger, IUserRepository userRepository) 
        {
            _logger = logger;
            _userRepository = userRepository;
        }
        public async Task<ConsumeResult<string, string>> ProcessEventAsync(string eventType, string key, string message)
        {
            ConsumeResult<string, string> result = null;
            if (eventType is string loginEvent)
            {
                try
                {
                    // Deserialize the JSON message to extract username and password
                    var register = JsonConvert.DeserializeObject<AccountRegistrationRequestDto>(message);

                    // Retrieve the user from the database by username
                    //var user = await _userRepository.GetUserByEmailAsync(loginData.Email);

                    
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    _logger.LogError($"Error processing login: {ex.Message}");
                }



                _logger.LogInformation($"Received login event: {loginEvent}");

                // Return a response if needed
                return await Task.FromResult(result);
            }
            else
            {
                _logger.LogWarning($"Received an invalid login event of type ");
                return result;
            }

        }
    }
}
