using Damaris.Kafka.Consumer.Models;

namespace Damaris.Kafka.Consumer.Telemetry
{
    /// <summary>
    /// Telemetry configuration options.
    /// </summary>
    public class TelemetryOptions
    {
        /// <summary>
        /// The time interval between invocations of <see cref="KafkaTelemetry.OnReported"/> events.
        /// Default reporting period is 1 minute.
        /// </summary>
        public TimeSpan ReportingPeriod { get; set; } = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Triggered when Kafka metrics are reported.
        /// Reporting schedule is configured during initialization using <see cref="ReportingPeriod"/>.
        /// Reporting period defaults to 1 minute.
        /// </summary>
        public event EventHandler<KafkaMetrics> OnReported;

        public event EventHandler<ConnectionEventArgs> OnError;

        public TelemetryOptions()
        {
            KafkaTelemetry.OnReported += (_, metrics) => OnReported?.Invoke(this, metrics);
            KafkaTelemetry.OnError += (_, args) => OnError?.Invoke(this, args);
        }
    }
}
