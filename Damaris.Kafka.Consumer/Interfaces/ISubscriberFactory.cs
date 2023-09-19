namespace Damaris.Kafka.Consumer.Interfaces
{
    public interface ISubscriberFactory
    {
        IEnumerable<ISubscriber> CreateSubscribers();
    }
}
