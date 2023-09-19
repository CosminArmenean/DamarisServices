using Damaris.Kafka.Consumer.Models;

namespace Damaris.Kafka.Consumer.Interfaces
{
    /// <summary>
    /// Defines a message handler
    /// </summary>
    public interface IMessageHandler : IDisposable
    {
        /// <summary>
        /// Processes a Kafka message. Any unhandled exceptions prevent storing offsets.
        /// </summary>
        /// <param name="message">Consumed Kafka message</param>
        Task ProcessMessageAsync(Message message);

        void IDisposable.Dispose()
        {
            // do nothing
        }
    }
}
