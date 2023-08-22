namespace KafkaCommunicationLibrary.Repositories.Interfaces
{
    public interface IKafkaConsumer
    {
        Task ConsumeAsync(string topic, Action<string> processMessage, CancellationToken cancellationToken);
        Task<string> WaitForResponse(string responseTopic, string key);
    }
}
