using Confluent.Kafka;
using KafkaCommunicationLibrary.Consumers;
using KafkaCommunicationLibrary.Producers;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IConfiguration configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
