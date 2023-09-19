namespace Damaris.Kafka.Consumer.Models
{
    /// <summary>
    /// Provides options that control the behavior of channels
    /// </summary>
    public class ChannelSettings
    {
        /// <summary>
        /// Channel behavior (or mode)
        /// </summary>
        public ChannelMode Mode { get; set; }

        /// <summary>
        /// The maximum number of items the bounded channel may store
        /// </summary>
        public int Capacity { get; set; } = 1;

        /// <summary>
        /// Indicates whether a single reader instance will consume the channel
        /// </summary>
        public bool SingleReader => Consumers == 1;

        /// <summary>
        /// Total amount of consumer instances of the internal channel
        /// </summary>
        public int Consumers { get; set; } = 1;
    }
}
