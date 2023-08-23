using Confluent.Kafka;
using DamarisServices.Utilities.v1;
using Damaris.Domain.v1.Models.User;
using Damaris.Domain.v1.Models.Account;
using Damaris.Domain.v1.Dtos.GenericDtos;
using KafkaCommunicationLibrary.Producers;
using KafkaCommunicationLibrary.Consumers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using DamarisServices.Utilities.v1.Generic;

namespace DamarisServices.Features.v1.User
{
    public class CreateTokenRequest : ApiRequest<DeliveryResult<string, string>>
    {
        public ApplicationUser User { get; init; }
        public ProducerRecord KafkaRecord { get; set; }
    }

    public class CreateTokenHandler : ApiRequestHandler<CreateTokenRequest, DeliveryResult<string, string>>
    {
        public CreateTokenHandler(KafkaProducer<string, string> producer, KafkaConsumer<string, string> consumer, ILoggerFactory logger) : base(producer, consumer, logger) { }

        public async override Task<DeliveryResult<string, string>> Handle(CreateTokenRequest request, CancellationToken cancellationToken)
        {
            // Log the request to the watchdog logger
            _logger.LogInformation("Sent request to Kafka: {request}", request);
            // Create a serializer instance
            //var serializer = SerializerFactory.Create<ApplicationUser>();

            // Serialize the message
            //var bytes = serializer.Serialize(request.User);
            //create a hash code based on message for identifier 
            string key = GenerateHashCode.GetHashCode(request.User.ToString());
            request.KafkaRecord.Key = key;
            // Send the message to Kafka
            DeliveryResult<string, string> response = null;
            var result = await _producer.Produce(request.KafkaRecord.Topic, request.KafkaRecord.Key, "First message produce by Identity Service!");
            if (result != null)
            {
                var processedData = _consumer.WaitForResponse("user-authentication-topic", request.KafkaRecord.Key);
            }
            return response;

        }
    }
}
