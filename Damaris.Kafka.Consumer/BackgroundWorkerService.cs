using Damaris.Kafka.Consumer.Interfaces;

namespace Damaris.Kafka.Consumer
{
    /// <summary>
    /// Implements a long running hosted service with Kafka subscribers
    /// </summary>
    public class BackgroundWorkerService : BackgroundService
    {
        private readonly IEnumerable<ISubscriber> _subscribers;

        /// <summary>
        /// Creates the long running hosted service
        /// </summary>
        public BackgroundWorkerService(ISubscriberFactory subscriberFactory)
        {
            _subscribers = subscriberFactory.CreateSubscribers();
        }

        /// <summary>
        /// Starts subscriber and waits for all to finish consumption
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.WhenAll(_subscribers.Select(s => s.StartAsync(stoppingToken)));
        }
    }
}
