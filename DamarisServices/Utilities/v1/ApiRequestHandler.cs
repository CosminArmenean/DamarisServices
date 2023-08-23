using Confluent.Kafka;
using DamarisServices.Features.v1.User;
using KafkaCommunicationLibrary.Consumers;
using KafkaCommunicationLibrary.Producers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace DamarisServices.Utilities.v1
{
    /// <summary>
    /// Base class for an api request handler
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class ApiRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : ApiRequest<TResponse>
        where TResponse : DeliveryResult<string, string>
    {
        protected readonly KafkaProducer<string, string> _producer;
        protected readonly KafkaConsumer<string, string> _consumer;
        protected readonly ILogger _logger;

        protected ApiRequestHandler(KafkaProducer<string, string> producer, KafkaConsumer<string, string> consumer, ILoggerFactory logger)
        {
            _producer = producer;
            _consumer = consumer;
            _logger = logger.CreateLogger(GetType());
        }

        /// <summary>
        /// This method handle the request
        /// To be implemented by the overloaded class
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>       
        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
        
    }

    
    
}

