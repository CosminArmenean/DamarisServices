using Confluent.Kafka;
using Damaris.DataService.Repositories.v1.Implementation.TopicEventsProcessor;
using Damaris.DataService.Repositories.v1.Interfaces.Contracts;
using Damaris.DataService.Services.v1.KafkaConsumer;
using KafkaCommunicationLibrary.Consumers;
using KafkaCommunicationLibrary.Producers;
using KafkaCommunicationLibrary.Repositories.Interfaces;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IConfiguration configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Register the topic event processors as scoped services:
builder.Services.AddScoped<Damaris.DataService.Repositories.v1.Interfaces.Contracts.IKafkaTopicEventProcessor, LoginEventProcessor>();


//Register the Kafka Consumer 
//var consumerConfig = new ConsumerConfig();
builder.Services.AddScoped<KafkaConsumer<string, string>>();
builder.Services.AddScoped<KafkaProducer<string, string>>();

//Add background services
builder.Services.AddHostedService<IdentityServiceConsumer>();

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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
