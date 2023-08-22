using Confluent.Kafka;
using DamarisServices.Utilities.v1;
using Damaris.Domain.v1.Models.User;
using Damaris.Domain.v1.Models.Account;
using Damaris.Domain.v1.Dtos.GenericDtos;
using KafkaCommunicationLibrary.Producers;
using KafkaCommunicationLibrary.Consumers;

namespace DamarisServices.Features.v1.User
{
    public class CreateTokenRequest : ApiRequest<DeliveryResult<string, string>>
    {
        public ApplicationUser User { get; init; }
        public ProducerRecord KafkaRecord { get; set; }
    }

    public class CreateTokenHandler : ApiRequestHandler<CreateTokenRequest, DeliveryResult<string, string>>
    {
        public CreateTokenHandler(KafkaProducer<string, string> producer, KafkaConsumer<string, string> consumer, ILogger watchdogLogger) : base(producer, consumer, (ILogger<ApiRequestHandler<CreateTokenRequest, DeliveryResult<string, string>>>)watchdogLogger) { }

        public async override Task<DeliveryResult<string, string>> Handle(CreateTokenRequest request, CancellationToken cancellationToken)
        {
            // Log the request to the watchdog logger
            _logger.LogInformation("Sent request to Kafka: {request}", request);
            // Create a serializer instance
            //var serializer = SerializerFactory.Create<ApplicationUser>();
                        
            // Serialize the message
            //var bytes = serializer.Serialize(request.User);

            // Create a Message object
            Message<string, string> message = new Message<string, string>() { Value = request.User.ToString() };

            // Send the message to Kafka
            DeliveryResult<string, string> response = null;
            _producer.Produce(request.KafkaRecord.Topic, message.Key, message.Value);
            return response;

        }
    }
}
