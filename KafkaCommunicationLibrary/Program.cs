using Confluent.Kafka;
using KafkaCommunicationLibrary.Consumers;
using KafkaCommunicationLibrary.Producers;
using Microsoft.Extensions.Configuration;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

IConfiguration configuration = builder.Configuration;

// Configure Kafka producer
builder.Services.AddSingleton(provider =>
{
    var kafkaConfig = configuration.GetSection("Kafka:Producer").Get<ProducerConfig>();
    return new KafkaProducer<string, string>(kafkaConfig, provider.GetService<ILogger<KafkaProducer<string, string>>>());
});

// Configure Kafka consumer
builder.Services.AddSingleton(provider =>
{
    var kafkaConfig = configuration.GetSection("Kafka:Consumer").Get<ConsumerConfig>();
    return new KafkaConsumer<string, string>(kafkaConfig, provider.GetService<ILogger<KafkaConsumer<string, string>>>());
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


app.Run();


