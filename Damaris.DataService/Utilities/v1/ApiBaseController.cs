using Confluent.Kafka;
using KafkaCommunicationLibrary.Producers;
using Microsoft.AspNetCore.Mvc;

namespace Damaris.DataService.Utilities.v1
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
        public ApiBaseController(IProducer<string, string> producer, IConsumer<string, string> consumer, ILogger<ApiBaseController> watchdogLogger)
        {
            _producer = producer;
            _consumer = consumer;
            _watchdogLogger = watchdogLogger;
        }
        /// <summary>
        /// Handles the controller request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        protected async Task HandleRequestAsync<T>(T request)
        {
            try
            {
                // Get the topic name from the request
                var topic = request.GetType().Name;

                //log the request
                string message = $"Receive request: {HttpContext.Request.Method} {HttpContext.Request.Path}";
                _watchdogLogger.LogInformation(message);

                // Create a Kafka record
                var record = "";// new ProducerRecord<string, string>(topic, request.ToString());

                // Send the record to Kafka
                // await _producer.ProduceAsync(record);
            }
            catch (Exception ex)
            {
                // Handle the exception
                _watchdogLogger.LogError(ex, "Failed to send request to Kafka");
            }
        }

    }
}
