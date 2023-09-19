namespace Damaris.Kafka.Consumer.Interfaces
{
    /// <summary>
    /// Defines a subscriber 
    /// </summary>
    public interface ISubscriber : IDisposable
    {
        /// <summary>
        /// Starts the subscriber
        /// </summary>
        /// <param name="stoppingToken">A cancellation token that can be used to cancel this operation</param>
        Task StartAsync(CancellationToken stoppingToken);
    }
}
