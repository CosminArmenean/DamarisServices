using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;

namespace DamarisServices.Utilities.v1
{
    /// <summary>
    /// Base class for an api request.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class ApiRequest<TResponse>  where TResponse : DeliveryResult<string, string> { }
}
