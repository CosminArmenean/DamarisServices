using App.Metrics;
using Confluent.Kafka;
using Damaris.Kafka.Consumer.Interfaces;
using Damaris.Kafka.Consumer.Models;
using Damaris.Kafka.Consumer.Subscribers;

namespace Damaris.Kafka.Consumer.Factories
{
    internal class SubscriberFactory : ISubscriberFactory
    {
        private readonly KafkaConsumerSettings _settings;
        private readonly INamedServiceResolver<IMessageHandler> _messageHandlerProvider;
        private readonly IMetricsRoot _metrics;
        private readonly ILogger<ISubscriber> _subscriberLogger;

        public SubscriberFactory(
            KafkaConsumerSettings settings,
            IMetricsRoot metrics,
            INamedServiceResolver<IMessageHandler> messageHandlerProvider,
            ILogger<ISubscriber> subscriberLogger)
        {
            _settings = settings;
            _metrics = metrics;
            _subscriberLogger = subscriberLogger;
            _messageHandlerProvider = messageHandlerProvider;
        }

        public IEnumerable<ISubscriber> CreateSubscribers()
        {
            foreach (var (id, consumer) in _settings.Consumers)
            {
                if (!_settings.Clusters.TryGetValue(consumer.ClusterId, out var matchedCluster))
                    throw new Exception($"Cluster with id {consumer.ClusterId} not found");

                // We need to clone the cluster settings to avoid changing the original settings
                // Then apply the consumer specific settings
                var cluster = (ClusterSettings)matchedCluster.Clone();
                cluster.GroupId = consumer.GroupId;

                if (consumer.PreserveOrder)
                {
                    yield return new OrderedKafkaSubscriber(consumer.Topic, consumer.Channel, () => _messageHandlerProvider(id), _metrics, () => new ConsumerBuilder<string, string>(cluster), _subscriberLogger);
                }
                else
                {
                    yield return new UnorderedKafkaSubscriber(consumer.Topic, consumer.Channel, () => _messageHandlerProvider(id), _metrics, () => new ConsumerBuilder<string, string>(cluster), _subscriberLogger);
                }
            }
        }
    }
}
