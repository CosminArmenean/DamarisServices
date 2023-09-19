namespace Damaris.Kafka.Consumer.Models
{
    /// <summary>
    /// Represents connections event data.
    /// </summary>
    public class ConnectionEventArgs : EventArgs
    {
        /// <summary>
        /// Exception that caused the event to be triggered.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionEventArgs"/> class.
        /// </summary>
        /// <param name="exception">Exception that caused the event to be triggered.</param>
        public ConnectionEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }
}
