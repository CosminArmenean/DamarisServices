namespace Damaris.Kafka.Consumer.Models
{
    /// <summary>
    /// Represents subscriber event data.
    /// </summary>
    public class SubscriberEventArgs : EventArgs
    {
        /// <inheritdoc cref="Models.Message"/>
        public Message Message { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriberEventArgs"/> class.
        /// </summary>
        /// <param name="message">A message consumed from a Kafka cluster.</param>
        public SubscriberEventArgs(Message message)
        {
            this.Message = message;
        }
    }
}
