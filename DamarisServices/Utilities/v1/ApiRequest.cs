using Confluent.Kafka;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DamarisServices.Utilities.v1
{
    /// <summary>
    /// Base class for an api request.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class ApiRequest<TResponse> : IRequest<TResponse> where TResponse : DeliveryResult<string, string> { }
}
