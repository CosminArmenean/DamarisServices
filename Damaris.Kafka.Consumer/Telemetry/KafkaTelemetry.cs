using App.Metrics.Timer;
using App.Metrics;
using Damaris.Kafka.Consumer.Models;

namespace Damaris.Kafka.Consumer.Telemetry
{
    /// <summary>
    /// Manages and reports Kafka telemetry. 
    /// </summary>
    public static class KafkaTelemetry
    {
        // ReSharper disable once NotAccessedField.Local
        // Used to keep timer running
        private static Timer _reportingTimer;

        internal static readonly TimerOptions ProcessingTimer = new TimerOptions
        {
            Context = "Application",
            Name = "Processing Timer",
            RateUnit = TimeUnit.Minutes,
            DurationUnit = TimeUnit.Milliseconds
        };

        internal static event EventHandler<KafkaMetrics> OnReported;

        internal static event EventHandler<ConnectionEventArgs> OnError;

        /// <summary>
        /// Gets a snapshot of Kafka metrics.
        /// </summary>
        public static KafkaMetrics GetMetrics()
        {
            var timerValue = Metrics.Instance?.Snapshot.GetForContext(ProcessingTimer.Context).Timers
                .FirstOrDefault(source => source.MultidimensionalName == ProcessingTimer.Name)?.Value;
            if (timerValue is null) return null;

            return new KafkaMetrics(
                new RatePerMinute(
                    timerValue.Rate.MeanRate,
                    timerValue.Rate.OneMinuteRate,
                    timerValue.Rate.FiveMinuteRate,
                    timerValue.Rate.FifteenMinuteRate),
                new ProcessingTime(
                    (long)Math.Truncate(timerValue.Histogram.Min),
                    (long)Math.Truncate(timerValue.Histogram.Max),
                    (long)Math.Truncate(timerValue.Histogram.Mean),
                    (long)Math.Truncate(timerValue.Histogram.StdDev),
                    (long)Math.Truncate(timerValue.Histogram.Percentile95),
                    (long)Math.Truncate(timerValue.Histogram.Percentile99)));
        }

        internal static void SetReportingSchedule(TelemetryOptions options)
        {
            options ??= new TelemetryOptions();
            _reportingTimer = new Timer(ReportMetrics, null, TimeSpan.Zero, options.ReportingPeriod);
        }

        private static void ReportMetrics(object state)
        {
            var handler = OnReported;
            if (handler != null)
            {
                var metrics = GetMetrics();
                if (metrics != null)
                {
                    handler.Invoke(null, metrics);
                }
            }
        }

        internal static void ReportError(Exception exception)
        {
            var handler = OnError;
            if (handler != null)
            {
                try
                {
                    handler.Invoke(null, new ConnectionEventArgs(exception));
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}
