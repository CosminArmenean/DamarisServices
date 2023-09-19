namespace Damaris.Kafka.Consumer.Telemetry
{
    /// <summary>
    /// Represents metrics captured during Kafka consumption.
    /// </summary>
    public sealed class KafkaMetrics : EventArgs
    {
        internal KafkaMetrics(RatePerMinute ratePerMinute, ProcessingTime processingTime)
        {
            RatePerMinute = ratePerMinute;
            ProcessingTime = processingTime;
        }

        /// <inheritdoc cref="Telemetry.RatePerMinute"/>
        public RatePerMinute RatePerMinute { get; }

        /// <inheritdoc cref="Telemetry.ProcessingTime"/>
        public ProcessingTime ProcessingTime { get; }
    }

    /// <summary>
    /// Represents the consumption rate per minute of Kafka messages.
    /// </summary>
    public sealed class RatePerMinute
    {
        internal RatePerMinute(double mean, double oneMinute, double fiveMinute, double fifteenMinute)
        {
            Mean = Math.Round(mean, 2);
            OneMinute = Math.Round(oneMinute, 2);
            FiveMinute = Math.Round(fiveMinute, 2);
            FifteenMinute = Math.Round(fifteenMinute, 2);
        }

        /// <summary>
        /// The average consumption rate for the lifetime of the application.
        /// Does not provide a sense of recency.
        /// </summary>
        public double Mean { get; }

        /// <summary>
        /// The last minute consumption rate.
        /// </summary>
        public double OneMinute { get; }

        /// <summary>
        /// The last five minutes consumption rate.
        /// </summary>
        public double FiveMinute { get; }

        /// <summary>
        /// The fifteen minutes consumption rate.
        /// </summary>
        public double FifteenMinute { get; }
    }

    /// <summary>
    /// Represents the processing time of Kafka consumption in milliseconds.
    /// Considers the lifetime of the application.
    /// </summary>
    public sealed class ProcessingTime
    {
        internal ProcessingTime(long min, long max, long mean, long stdDev, long perc95, long perc99)
        {
            Min = min;
            Max = max;
            Mean = mean;
            StdDev = stdDev;
            Perc95 = perc95;
            Perc99 = perc99;
        }

        /// <summary>
        /// The minimum processing time in milliseconds.
        /// </summary>
        public long Min { get; }

        /// <summary>
        /// The maximum processing time in milliseconds.
        /// </summary>
        public long Max { get; }

        /// <summary>
        /// The mean (or average) processing time in milliseconds.
        /// </summary>
        public long Mean { get; }

        /// <summary>
        /// The standard deviation of the processing time in milliseconds.
        /// </summary>
        public long StdDev { get; }

        /// <summary>
        /// The 95th percentile of the processing time in milliseconds.
        /// </summary>
        public long Perc95 { get; }

        /// <summary>
        /// The 99th percentile of the processing time in milliseconds.
        /// </summary>
        public long Perc99 { get; }
    }
}
