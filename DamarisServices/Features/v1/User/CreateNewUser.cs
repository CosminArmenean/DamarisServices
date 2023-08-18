using Confluent.Kafka;
using DamarisServices.Utilities.v1;
using Microsoft.AspNetCore.Mvc;
using Damaris.Domain.v1.Dtos.GenericDtos;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Org.BouncyCastle.Crypto.Tls;

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
        public CreateNewUserHandler(IProducer<string, string> producer, ILogger watchdogLogger) : base(producer, (ILogger<ApiRequestHandler<CreateNewUserRequest, DeliveryResult<string, string>>>)watchdogLogger) { }
               
        public async override Task<DeliveryResult<string, string>> Handle(CreateNewUserRequest request, CancellationToken cancellationToken)
        {
            // Log the request to the watchdog logger
            _watchdogLogger.LogInformation("Sent request to Kafka: {request}", request);
            // Create a Message object
            Message<string, string> message = new Message<string, string>() {  Key = request.Payload.Key, Value = request.Payload.Value };

            // Send the message to Kafka
            DeliveryResult<string, string> response = await _producer.ProduceAsync(request.Payload.Topic, message, cancellationToken);
            return response;                  

        }
    }
}