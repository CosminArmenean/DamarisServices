using Damaris.Kafka.Consumer.Factories;
using Damaris.Kafka.Consumer.Interfaces;
using Damaris.Kafka.Consumer.Models;
using Damaris.Kafka.Consumer.Telemetry;
using System.Diagnostics.CodeAnalysis;

namespace Damaris.Kafka.Consumer.Extensions
{
    /// <summary>
    /// Provides extension methods to register interfaces to default implementations.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers all interfaces to their default implementation.
        /// </summary>
        /// <param name="services">The contract for a collection of service descriptors.</param>
        /// <param name="configuration">A set of key/value application configuration properties.</param>
        /// <param name="options">Options to define message handlers and telemetry</param>
        public static void AddKafkaConsumer(this IServiceCollection services, IConfiguration configuration, Action<KafkaConsumerSettings> options)
        {
            var settings = configuration.GetSection(nameof(KafkaConsumerSettings)).Get<KafkaConsumerSettings>() ?? new KafkaConsumerSettings();
            options?.Invoke(settings);
            services.AddSingleton(settings);

            var logger = services.BuildServiceProvider().GetRequiredService<ILogger<ClusterSettings>>();
            foreach (var cluster in settings.Clusters.Values)
            {
                Setup.ConfigureCredentials(cluster, logger);
            }

            foreach (var (id, consumer) in settings.Consumers)
            {
                if (consumer.MessageHandlerType == null)
                    throw new Exception($"Message handler type not defined for consumer '{id}'. Make sure you call AddHandler<T> for consumer '{id}'.");

                // Message handlers are registered as transient so that the Kafka Subscriber can create a new instance for each consumer
                // Creating a new instance is required because we cannot guarantee the message handler thread safety
                services.AddNamedService<IMessageHandler>(consumer.MessageHandlerType, id);
            }

            services
                .AddSingleton<ISubscriberFactory, SubscriberFactory>()
                .AddHostedService<BackgroundWorkerService>()
                .AddMetrics();

            KafkaTelemetry.SetReportingSchedule(settings.Telemetry);
        }
    }
}
