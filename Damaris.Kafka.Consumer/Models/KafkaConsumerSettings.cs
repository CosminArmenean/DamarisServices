using Damaris.Kafka.Consumer.Interfaces;
using Damaris.Kafka.Consumer.Telemetry;

namespace Damaris.Kafka.Consumer.Models
{
    public class KafkaConsumerSettings
    {
        public Dictionary<string, ClusterSettings> Clusters { get; set; } = new();
        public Dictionary<string, ConsumerSettings> Consumers { get; set; } = new();
        public TelemetryOptions Telemetry { get; set; } = new();

        /// <summary>
        /// Adds a message handler to the consumer
        /// </summary>
        /// <typeparam name="TMessageHandler">The message handler type.</typeparam>
        /// <param name="consumerIds">The consumer id as set in the configuration. If multiple ids are passed, they all bind to the same handler.</param>
        /// <exception cref="KeyNotFoundException">Thrown if the consumer id is not found in the configuration.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the same consumer id is configured multiple times.</exception>
        public void AddHandler<TMessageHandler>(params string[] consumerIds) where TMessageHandler : class, IMessageHandler
        {
            if (consumerIds == null || consumerIds.Length == 0)
                throw new ArgumentException(@"At least one consumer id must be provided", nameof(consumerIds));

            foreach (var consumerId in consumerIds.Distinct())
            {
                if (!Consumers.TryGetValue(consumerId, out var consumer))
                    throw new KeyNotFoundException($"Consumer with id {consumerId} not found");

                if (consumer.MessageHandlerType != null)
                    throw new InvalidOperationException($"Message handler type already defined for consumer {consumerId}");

                consumer.MessageHandlerType = typeof(TMessageHandler);
            }
        }
    }

    public class ConsumerSettings
    {
        public string ClusterId { get; set; }
        public string GroupId { get; set; }
        public string Topic { get; set; }
        public bool PreserveOrder { get; set; }
        public ChannelSettings Channel { get; set; } = new();
        public Type MessageHandlerType { get; set; }
    }
}
