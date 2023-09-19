using Damaris.Kafka.Consumer.Models;
using System.ComponentModel;
using System.Threading.Channels;

namespace Damaris.Kafka.Consumer.Factories
{
    /// <summary>
    /// Implements a factory responsible for creating channels
    /// </summary>
    public class ChannelFactory
    {
        public static Channel<Message> CreateChannel(ChannelSettings settings)
        {
            return settings.Mode switch
            {
                ChannelMode.Bounded => CreateBoundedChannel(settings),
                ChannelMode.Unbounded => CreateUnboundedChannel(settings),
                _ => throw new InvalidEnumArgumentException($@"Invalid Channel Mode: {settings.Mode}")
            };
        }

        private static Channel<Message> CreateBoundedChannel(ChannelSettings settings)
        {
            var options = new BoundedChannelOptions(settings.Capacity)
            {
                SingleWriter = true,
                SingleReader = settings.SingleReader,
                FullMode = BoundedChannelFullMode.Wait
            };

            return Channel.CreateBounded<Message>(options);
        }

        public static Channel<Message> CreateUnboundedChannel(ChannelSettings settings)
        {
            var options = new UnboundedChannelOptions
            {
                SingleWriter = true,
                SingleReader = settings.SingleReader
            };

            return Channel.CreateUnbounded<Message>(options);
        }
    }
}
