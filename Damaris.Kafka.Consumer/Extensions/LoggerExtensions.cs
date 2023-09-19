using Confluent.Kafka;
using System.Diagnostics.CodeAnalysis;

namespace Damaris.Kafka.Consumer.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class LoggerExtensions
    {
        public static void LogKafkaMessage(this ILogger logger, LogMessage message)
        {
            LogLevel level;
            switch (message.Level)
            {
                case SyslogLevel.Emergency:
                case SyslogLevel.Alert:
                case SyslogLevel.Critical:
                    level = LogLevel.Critical; break;
                case SyslogLevel.Error:
                    level = LogLevel.Error; break;
                case SyslogLevel.Warning:
                    level = LogLevel.Warning; break;
                case SyslogLevel.Notice:
                case SyslogLevel.Info:
                    level = LogLevel.Information; break;
                case SyslogLevel.Debug:
                    level = LogLevel.Debug; break;
                default:
                    level = LogLevel.Trace; break;
            }

            logger.Log(level, message.Message);
        }
    }
}
