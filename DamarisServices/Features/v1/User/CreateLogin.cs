using Confluent.Kafka;
using Damaris.Domain.v1.Dtos.GenericDtos;
using Damaris.Domain.v1.Models.Account;
using DamarisServices.Utilities.v1.Generic;
using DamarisServices.Utilities.v1;
using KafkaCommunicationLibrary.Consumers;
using KafkaCommunicationLibrary.Producers;
using Damaris.Domain.v1.Models.User;
using Newtonsoft.Json;

namespace DamarisServices.Features.v1.User
{
    public class CreateLoginRequest : ApiRequest<DeliveryResult<string, string>>
    {
        public LoginRequest LoginRequest { get; init; }
    }
    public class LoginRequestHandler : ApiRequestHandler<CreateLoginRequest, DeliveryResult<string, string>>
    {
        private readonly string Topic = "user-authentication-topic";
        private readonly string ResponseTopic = "user-authentication-response-topic";
        public LoginRequestHandler(KafkaProducer<string, string> producer, KafkaConsumer<string, string> consumer, ILoggerFactory logger) : base(producer, consumer, logger) { }

        public async override Task<DeliveryResult<string, string>> Handle(CreateLoginRequest request, CancellationToken cancellationToken)
        {
            // Log the request to the watchdog logger
            _logger.LogInformation("Preparing request to Kafka: {request}", request);

            // Serialize the login event to JSON
            var jsonLoginEvent = JsonConvert.SerializeObject(request);

            //create a hash code based on email/username for identifier 
            string key = GenerateHashCode.GetHashCode(request.LoginRequest.Email.ToString());
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
