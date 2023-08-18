using Confluent.Kafka;
using Damaris.DataService.Utilities.v1;
using Damaris.Domain.v1.Dtos.GenericDtos;
using Microsoft.AspNetCore.Mvc;

namespace Damaris.DataService.Controllers.v1
{
    public class IdentityController : ApiBaseController
    {
        private readonly IProducer<string, string> _producer;
        private readonly IConsumer<string, string> _consumer;
        private readonly ILogger<ApiBaseController> _watchLogger;
        public IdentityController(IProducer<string, string> producer, IConsumer<string, string> consumer, ILogger<ApiBaseController> logger) : base(producer, consumer, logger)
        {
            _producer = producer;
            _consumer = consumer;
        }

        [HttpPost("/process-register")]
        public async Task ProcessRegisterAsync()
        {
            try
            {
                // Create a ConsumerRecord object
                //var consumerRecord = new ConsumerRecord<string, string>("topic", 0, 0, "dummy data");

                // Consume the message from Kafka
               // var task = await _consumer.ConsumeAsync(consumerRecord);

                //// Do something with the message
                //if (task.IsCompletedSuccessfully)
                //{
                //    // The message was consumed successfully
                //}
                //else
                //{
                //    // The message was not consumed successfully
                //}
                // Process the register request from Kafka
                //var record = await _consumer.ConsumeAsync();

                //var userId = record.Key;
                //var password = record.Value;

                // Do something with the register request
                // ...
            }
            catch (Exception ex)
            {
                // Handle the exception
                _watchLogger.LogError(ex, "Failed to process register request");
            }
        }
    }
}
