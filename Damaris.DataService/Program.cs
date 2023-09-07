using Confluent.Kafka;
using Damaris.DataService.Data.v1;
using Damaris.DataService.Domain.v1.Models.Generic;
using Damaris.DataService.Repositories.v1.Implementation.TopicEventsProcessor;
using Damaris.DataService.Repositories.v1.Interfaces.UserInterfaces;
using Damaris.DataService.Services.v1.KafkaConsumer;
using KafkaCommunicationLibrary.Consumers;
using KafkaCommunicationLibrary.Domain.Models;
using KafkaCommunicationLibrary.Producers;
using KafkaCommunicationLibrary.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Reflection;
using WatchDog;
using WatchDog.src.Enums;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
IConfiguration configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();

// Bind configuration to classes
//AppSettings? appSettings = new() {  };
var OfficerMySqlGe2 = builder.Configuration.GetConnectionString(name: "OfficerMySqlGe2");
var WatchDogPostGreSql = builder.Configuration.GetConnectionString(name: "WatchDogPostGreSql");
//var appSettings = builder.Configuration.GetSection("ConnectionStrings").Get<AppSettings>();
//builder.Services.AddSingleton(appSettings);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add WatchDog Logger
builder.Services.AddLogging(logging => logging.AddWatchDogLogger());

builder.Services.AddWatchDogServices(opt =>
{
    opt.IsAutoClear = false;
    //opt.ClearTimeSchedule = WatchDogAutoClearScheduleEnum.Monthly;

    opt.SetExternalDbConnString = WatchDogPostGreSql;
    opt.DbDriverOption = WatchDogDbDriverEnum.PostgreSql;
});

//Initializing my DbContext inside the DI Container
builder.Services.AddDbContext<OfficerDbContext>(options => options.UseMySql(OfficerMySqlGe2, ServerVersion.AutoDetect(OfficerMySqlGe2)));


// Kafka settings
KafkaSettings kafkaSettings = builder.Configuration.GetSection("Kafka").Get<KafkaSettings>();
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

builder.Services.AddScoped<IUserRepository, IUserRepository>();
//builder.Services.AddHostedService<IdentityServiceConsumer>();

//builder.Services.AddScoped<KafkaConsumer<string, string>>();
builder.Services.AddScoped<KafkaProducer<string, string>>();

//Add background services
//builder.Services.AddScoped<IdentityServiceConsumer>();
// Change scoped to singleton if appropriate
//builder.Services.AddScoped<KafkaConsumer<string, string>>();
//builder.Services.AddScoped<IKafkaConsumer<string, string>, KafkaConsumer<string, string>>();
//Register the topic event processors as scoped services:
builder.Services.AddSingleton<IKafkaTopicEventProcessor<string, string>, LoginEventProcessor>();

builder.Services.AddHostedService<IdentityServiceConsumer>();
builder.Services.AddSingleton<IdentityServiceConsumer>();
builder.Services.AddSingleton<KafkaConsumer<string, string>>();



//Add background services
//builder.Services.AddSingleton<IHostedService, IdentityServiceConsumer>();

//builder.Services.AddSingleton<IKafkaConsumer<string, string>, KafkaConsumer<string, string>>();
//builder.Services.AddSingleton<IKafkaConsumer>(provider => provider.GetRequiredService<IdentityServiceConsumer>());



//Add MediatR           
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//inject WatchDog Logger to the middleware 
app.UseWatchDogExceptionLogger();
//This authentication information (Username and Password) will be used to access the log viewer.
app.UseWatchDog(opt =>
{
    opt.WatchPageUsername = "admin";
    opt.WatchPagePassword = "Qwerty@123";
});

app.MapControllers();





app.Run();
