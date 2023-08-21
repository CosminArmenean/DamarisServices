using Confluent.Kafka;
using Damaris.Domain.v1.Dtos.GenericDtos;
using KafkaCommunicationLibrary.Producers;
using Microsoft.AspNetCore.Mvc;

namespace DamarisServices.Utilities.v1
{
    public abstract class ApiBaseController : Controller
    {
        private readonly IProducer<string, string> _producer;
        private readonly IConsumer<string, string> _consumer;
        private readonly ILogger<ApiBaseController> _watchdogLogger;

        /// <summary>
        /// Base class for an MVC controller with view support.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="loggerFactory"></param>
        public ApiBaseController(IProducer<string, string> producer, ILogger<ApiBaseController> watchdogLogger)
        {
            _producer = producer;
            _watchdogLogger = watchdogLogger;
        }
        /// <summary>
        /// Handles the controller request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        protected async Task<DeliveryResult<string, string>> HandleRequestAsync<T>(ApiRequest<T> request) where T : DeliveryResult<string, string>
        {
            try
            {
                // Get the topic name from the request
                var topic = request.GetType().Name;

                //log the request
                string messagelog = $"Receive request: {HttpContext.Request.Method} {HttpContext.Request.Path}";
                _watchdogLogger.LogInformation(messagelog);

                // Create a Kafka record
                var record = new ProducerRecord() { Topic = topic , Value = request.ToString() };
                // Create a Message object
                Message<string, string> message = new Message<string, string>() { Key = "",  Value = "Test" };

                // Send the record to Kafka
                DeliveryResult<string, string> response = await _producer.ProduceAsync(record.Topic, message);
                // Consume the response from Kafka
                

                return response;
            }
            catch (Exception ex)
            {
                // Handle the exception
                _watchdogLogger.LogError(ex, "Failed to send request to Kafka");
                return null;
            }
        }

    }
}

