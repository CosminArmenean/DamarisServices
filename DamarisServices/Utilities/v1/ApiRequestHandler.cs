using Confluent.Kafka;
using DamarisServices.Features.v1.User;
using Microsoft.AspNetCore.Mvc;
using static Confluent.Kafka.ConfigPropertyNames;

namespace DamarisServices.Utilities.v1
{
    /// <summary>
    /// Base class for an api request handler
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class ApiRequestHandler<TRequest, TResponse> where TRequest : ApiRequest<TResponse>
        where TResponse : DeliveryResult<string, string>
    {
        protected readonly IProducer<string, string> _producer;
        protected readonly ILogger<ApiRequestHandler<TRequest, TResponse>> _watchdogLogger;

        protected ApiRequestHandler(IProducer<string, string> producer, ILogger<ApiRequestHandler<TRequest, TResponse>> watchdogLogger)
        {
            _producer = producer;
            _watchdogLogger = watchdogLogger;
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

