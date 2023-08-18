using KafkaCommunicationLibrary.Consumers;
using KafkaCommunicationLibrary.Producers;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.





var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();





app.Run();


