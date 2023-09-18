using Confluent.Kafka;
using KafkaCommunicationLibrary.Consumers;
using KafkaCommunicationLibrary.Producers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Damaris.Officer.Utilities.v1
{
    public abstract class ApiBaseController : Controller
    {
        private readonly KafkaProducer<string, string> _producer;
        private readonly KafkaConsumer<string, string> _consumer;
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        /// <summary>
        /// Base class for an MVC controller with view support.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="loggerFactory"></param>
        public ApiBaseController(IMediator mediator, KafkaProducer<string, string> producer, KafkaConsumer<string, string> consumer, ILoggerFactory loggerFactory)
        {
            _producer = producer;
            _consumer = consumer;
            _logger = loggerFactory.CreateLogger(GetType());
            _mediator = mediator;
        }
        /// <summary>
        /// Handles the controller request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        protected async Task<IActionResult> HandleRequestAsync<T>(ApiRequest<T> request) where T : DeliveryResult<string, string>
        {
            string message = $"Receive request: {HttpContext.Request.Method} {HttpContext.Request.Path}";
            _logger.LogInformation(message);
            return (IActionResult)await _mediator.Send(request);
            //try
            //{
            //    // Get the topic name from the request
            //    var topic = "user-authentication-topic";

            //    //log the request
            //    string messagelog = $"Receive request: {HttpContext.Request.Method} {HttpContext.Request.Path}";
            //    _watchdogLogger.LogInformation(messagelog);

            //    // Create a Kafka record
            //    var record = new ProducerRecord() { Topic = topic , Value = request.ToString() };
            //    // Create a Message object
            //    Message<string, string> message = new Message<string, string>() { Key = record.Key,  Value = "Test123" };

            //    // Send the record to Kafka
            //    DeliveryResult<string, string> response = null;
            //     _producer.Produce(record.Topic, message.Key, message.Value);
            //    // Consume the response from Kafka                

            //    return response;
            //}
            //catch (Exception ex)
            //{
            //    // Handle the exception
            //    _watchdogLogger.LogError(ex, "Failed to send request to Kafka");
            //    return null;
            //}
        }

    }
}
