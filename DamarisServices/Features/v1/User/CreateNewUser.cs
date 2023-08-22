using Confluent.Kafka;
using DamarisServices.Utilities.v1;
using Microsoft.AspNetCore.Mvc;
using Damaris.Domain.v1.Dtos.GenericDtos;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Org.BouncyCastle.Crypto.Tls;
using KafkaCommunicationLibrary.Producers;
using DamarisServices.Utilities.v1.Generic;
using KafkaCommunicationLibrary.Consumers;

namespace DamarisServices.Features.v1.User
{
    /// <summary>
    /// Request class for crating new user
    /// </summary>
    public class CreateNewUserRequest : ApiRequest<DeliveryResult<string, string>>
    {
        public string UserId { get; init; }
        public string Password { get; init; }
        public ProducerRecord Payload { get; set;}
    }

    public class CreateNewUserHandler : ApiRequestHandler<CreateNewUserRequest, DeliveryResult<string, string>>
    {
        public CreateNewUserHandler(KafkaProducer<string, string> producer, KafkaConsumer<string, string> consumer, ILogger logger) : base(producer, consumer, logger) { }
               
        public async override Task<DeliveryResult<string, string>> Handle(CreateNewUserRequest request, CancellationToken cancellationToken)
        {
            request.Payload.Topic = "user-authentication-topic";
            // Log the request to the watchdog logger
            _logger.LogInformation("Sent request to Kafka: {request}", request);
            // Create a Message object
            string key = GenerateHashCode.GetHashCode("test");
            request.Payload.Key = key;
            Message<string, string> message = new Message<string, string>() {  Key = request.Payload.Key, Value = request.Payload.Value };

            // Send the message to Kafka
            DeliveryResult<string, string> response = null;
            var result = await _producer.Produce(request.Payload.Topic, message.Key, message.Value);
            if (result != null)
            {
                var processedData = _consumer.WaitForResponse("user-authentication-topic", key);
            }
            return response;                  

        }
    }
}