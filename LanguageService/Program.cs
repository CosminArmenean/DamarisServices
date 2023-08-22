using KafkaCommunicationLibrary.Consumers;
using KafkaCommunicationLibrary.Producers;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Register the Kafka Consumer and producer
builder.Services.AddScoped<KafkaConsumer<string, string>>();
builder.Services.AddScoped<KafkaProducer<string, string>>();




var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();





app.Run();


