using Confluent.Kafka;
using KafkaCommunicationLibrary.Consumers;
using KafkaCommunicationLibrary.Domain.Models;
using KafkaCommunicationLibrary.Producers;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using Silverback.Messaging.Configuration;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

IConfiguration configuration = builder.Configuration;




// Kafka settings
KafkaSettings kafkaSettings = configuration.GetSection("Kafka").Get<KafkaSettings>();
builder.Services.AddSingleton(kafkaSettings);

// Kafka ConsumerConfig
ConsumerConfig consumerConfig = new ConsumerConfig
{
    BootstrapServers = kafkaSettings.BootstrapServers,
    GroupId = kafkaSettings.Consumer.GroupId,
    AutoOffsetReset = kafkaSettings.Consumer.AutoOffsetReset,
    EnableAutoCommit = kafkaSettings.Consumer.EnableAutoCommit
    // Other consumer configuration options
};
builder.Services.AddSingleton(consumerConfig);

// Kafka ProducerConfig
ProducerConfig producerConfig = new ProducerConfig
{
    BootstrapServers = kafkaSettings.BootstrapServers,
    Acks = Acks.None,   
    RetryBackoffMs = kafkaSettings.Producer.RetriesBackoffMs,
    MaxInFlight = kafkaSettings.Producer.MaxInFlight,
    EnableDeliveryReports = true,
    DeliveryReportFields = "key",
    Partitioner = Partitioner.Consistent
    
    // Other producer configuration options
};
builder.Services.AddSingleton(producerConfig);

// Register the Kafka Consumer and producer
builder.Services.AddScoped<KafkaConsumer<string, string>>();
builder.Services.AddScoped<KafkaProducer<string, string>>();



//builder.Services.AddSingleton(provider =>
//{
//    var kafkaConfig = configuration.GetSection("Kafka:Producer").Get<ProducerConfig>();
//    return new KafkaProducer<string, string>(kafkaConfig, provider.GetService<ILogger<KafkaProducer<string, string>>>());
//});

//// Configure Kafka consumer
//builder.Services.AddSingleton(provider =>
//{
//    var kafkaConfig = configuration.GetSection("Kafka:Consumer").Get<ConsumerConfig>();
//    return new KafkaConsumer<string, string>(kafkaConfig, provider.GetService<ILogger<KafkaConsumer<string, string>>>());
//});


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


app.Run();


