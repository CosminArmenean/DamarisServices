using Confluent.Kafka;
using Damaris.Frontier.Utilities.v1;
using Microsoft.AspNetCore.Mvc;
using Damaris.Domain.v1.Dtos.GenericDtos;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Org.BouncyCastle.Crypto.Tls;
using KafkaCommunicationLibrary.Producers;
using Damaris.Frontier.Utilities.v1.Generic;
using KafkaCommunicationLibrary.Consumers;
using Damaris.Domain.v1.Dtos.Requests.Account;
using Newtonsoft.Json;

namespace Damaris.Frontier.Features.v1.User
{
    /// <summary>
    /// Request class for crating new user
    /// </summary>
    public class CreateNewUserRequest : ApiRequest<DeliveryResult<string, string>>
    {
        public AccountRegistrationRequestDto AccountRegistration { get; init; }        
        
    }

    public class CreateNewUserHandler : ApiRequestHandler<CreateNewUserRequest, DeliveryResult<string, string>>
    {
        private readonly string Topic = "user-registration-topic";
        private readonly string ResponseTopic = "user-registration-response-topic";
        public CreateNewUserHandler(KafkaProducer<string, string> producer, KafkaConsumer<string, string> consumer, ILoggerFactory logger) : base(producer, consumer, logger) { }
               
        public async override Task<DeliveryResult<string, string>> Handle(CreateNewUserRequest request, CancellationToken cancellationToken)
        {
            // Log the request to the watchdog logger
            _logger.LogInformation("Preparing request to Kafka: {request}", request);

            // Serialize the login event to JSON
            var jsonLoginEvent = JsonConvert.SerializeObject(request);

            //create a hash code based on email/username for identifier 
            string key = GenerateHashCode.GetHashCode(request.AccountRegistration.Accounts[0].Email.ToString());
            //request.KafkaRecord.Key = key;

            //Create a object to store the response
            DeliveryResult<string, string> response = null;

            // Send the message to Kafka
            bool messageProduced = await _producer.Produce(Topic, key, jsonLoginEvent);
            if (messageProduced == true)
            {
                //consuming message from kafka topic response specifiyng the identifier
                var processedData = _consumer.WaitForResponse(ResponseTopic, key);
            }
            return response;

        }
    }
}