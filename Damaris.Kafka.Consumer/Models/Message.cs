using Confluent.Kafka;

namespace Damaris.Kafka.Consumer.Models
{
    /// <summary>
    /// Represents a message consumed from a Kafka cluster.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="raw">The raw Kafka <see cref="ConsumeResult{TKey,TValue}"/> value.</param>
        public Message(ConsumeResult<string, string> raw)
        {
            Raw = raw;
        }

        /// <summary>
        /// Gets the message key (possibly null).
        /// </summary>
        public string Key => Raw?.Message?.Key;

        /// <summary>
        /// Gets the message value (possibly null).
        /// </summary>
        public string Body => Raw?.Message?.Value;

        /// <summary>
        /// A collection of Kafka message headers.
        /// </summary>
        public Headers Headers => Raw?.Message?.Headers;

        /// <summary>
        /// The topic associated with the message.
        /// </summary>
        public string Topic => Raw?.Topic;

        /// <summary>
        /// Represents a raw message consumed from a Kafka cluster including metadata.
        /// </summary>
        public ConsumeResult<string, string> Raw { get; }
    }
}
