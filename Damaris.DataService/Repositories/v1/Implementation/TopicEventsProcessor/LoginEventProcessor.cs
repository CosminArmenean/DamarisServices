using Confluent.Kafka;
using Damaris.DataService.Repositories.v1.Interfaces.UserInterfaces;
using Damaris.Domain.v1.Models.User;
using Google.Protobuf.WellKnownTypes;
using KafkaCommunicationLibrary.Repositories.Interfaces;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Tls;

namespace Damaris.DataService.Repositories.v1.Implementation.TopicEventsProcessor
{
    public class LoginEventProcessor : IKafkaTopicEventProcessor<string, string>
    {
        private readonly ILogger<LoginEventProcessor> _logger;
        private readonly IUserRepository _userRepository;
        private readonly string IDENTITY_AUTHENTICATION_TOPIC = "user-authentication-topic";
        private readonly string IDENTITY_AUTHENTICATION_RESPONSE_TOPIC = "user-authentication-response-topic";
        private readonly string DATA = "LOGIN";
        public string Topic => IDENTITY_AUTHENTICATION_TOPIC;
        public string ResponseTopic => IDENTITY_AUTHENTICATION_RESPONSE_TOPIC;
        public string Data => DATA;

        public LoginEventProcessor(ILogger<LoginEventProcessor> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }
        public async Task<ConsumeResult<string, string>> ProcessEventAsync(string key, string message)
        {
            ConsumeResult<string, string> result = null;
          
                try
                {
                    // Deserialize the JSON message to extract username and password
                    var loginData = JsonConvert.DeserializeObject<LoginRequest>(message);

                    // Retrieve the user from the database by username
                    var user = await _userRepository.GetUserByEmailAsync(loginData.Email);

                    if (user != null && VerifyPassword(loginData.Password, ""))
                    {
                        // Authentication successful
                        result.Message.Value = "Authentication successful";
                        return result;
                    }
                    else
                    {
                        // Authentication failed
                        result.Message.Value = "Authentication failed";
                        return result;
                    }
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

        private bool VerifyPassword(string inputPassword, string storedPasswordHash)
        {
            // Implement password hashing and comparison logic here
            // You should use a secure password hashing library like BCrypt or Identity's PasswordHasher
            // For demonstration purposes, we'll use a simple string comparison (not secure)
            return inputPassword == storedPasswordHash;
        }
    }
}
