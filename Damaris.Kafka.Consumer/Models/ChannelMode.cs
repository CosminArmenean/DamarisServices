namespace Damaris.Kafka.Consumer.Models
{
    /// <summary>
    /// Provides options to determine the behavior of channels
    /// </summary>
    public enum ChannelMode
    {
        /// <summary>
        /// A bounded (limited) channel
        /// </summary>
        Bounded,
        /// <summary>
        /// An unbounded (limitless) channel
        /// </summary>
        Unbounded
    }
}
