using Confluent.Kafka;
using Damaris.DataService.Data.v1;
using Damaris.DataService.Domain.v1.Models.Generic;
using Damaris.DataService.MappingProfiles.v1;
using Damaris.DataService.Providers.v1.User;
using Damaris.DataService.Repositories.v1;
using Damaris.DataService.Repositories.v1.BusinessRoutines;
using Damaris.DataService.Repositories.v1.Implementation.TopicEventsProcessor;
using Damaris.DataService.Repositories.v1.Implementation.UserImplementation;
using Damaris.DataService.Repositories.v1.Interfaces.Generic;
using Damaris.DataService.Repositories.v1.Interfaces.UserInterfaces;
using Damaris.DataService.Services.v1.KafkaConsumer;
using Damaris.DataService.Services.v1.User;
using Damaris.Domain.v1.Models.Account;
using Damaris.Domain.v1.Models.User;
using IdentityServer4.Services;
using KafkaCommunicationLibrary.Consumers;
using KafkaCommunicationLibrary.Domain.Models;
using KafkaCommunicationLibrary.Producers;
using KafkaCommunicationLibrary.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql.Internal;
using System.Configuration;
using System.Reflection;
using System.Text;
using WatchDog;
using WatchDog.src.Enums;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
IConfiguration configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();

// Bind configuration to classes
//AppSettings? appSettings = new() {  };
var DamarisMySqlReadWrite = builder.Configuration.GetConnectionString(name: "DamarisMySqlReadWrite");
var DamarisMySqlReadOnly = builder.Configuration.GetConnectionString(name: "DamarisMySqlReadOnly");

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

//Initialize AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

//Initializing my DbContext inside the DI Container
builder.Services.AddDbContext<DamarisDbContext>(options => 
{
    options.UseMySql(DamarisMySqlReadWrite, ServerVersion.AutoDetect(DamarisMySqlReadWrite));
 }, ServiceLifetime.Singleton);
//add read only db context
builder.Services.AddDbContext<OfficerReadOnlyDbContext>(options =>
{
    options.UseMySql(DamarisMySqlReadOnly, ServerVersion.AutoDetect(DamarisMySqlReadOnly));
}, ServiceLifetime.Singleton);


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

//builder.Services.AddScoped<IUserRepository, IUserRepository>();
//builder.Services.AddHostedService<IdentityServiceConsumer>();

//builder.Services.AddScoped<KafkaConsumer<string, string>>();

//builder.Services.AddScoped<IdentityServiceConsumer>();
// Change scoped to singleton if appropriate
//builder.Services.AddScoped<KafkaConsumer<string, string>>();
//builder.Services.AddScoped<IKafkaConsumer<string, string>, KafkaConsumer<string, string>>();
//Register the topic event processors as scoped services:

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();

builder.Services.AddSingleton<IKafkaTopicEventProcessor<string, string>, LoginEventProcessor>();
builder.Services.AddSingleton<IKafkaTopicEventProcessor<string, string>, RegisterEventProcessor>();

builder.Services.AddHostedService<IdentityServiceConsumer>();
//builder.Services.AddSingleton<IdentityServiceConsumer>();

builder.Services.AddHostedService<AuthenticationServiceConsumer>();
//builder.Services.AddSingleton<AuthenticationServiceConsumer>();

builder.Services.AddSingleton<KafkaConsumer<string, string>>();
builder.Services.AddSingleton<KafkaProducer<string, string>>();


//Adding mediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
//==========================================

//builder.Services.AddSingleton<IProfileService, IdentityServerServiceProvider>();

//Getting the secret from the config
byte[] key = Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"]);



//Adding identity
//builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    //.AddEntityFrameworkStores<OfficerDbContext>()
    //.AddDefaultTokenProviders();


// Add IdentityServer4 to the services.
//builder.Services.AddIdentityServer()
//    .AddInMemoryClients(Config.Clients)
//    .AddInMemoryIdentityResources(Config.IdentityResources)
//    .AddInMemoryApiScopes(Config.ApiScopes);

//builder.Services.AddIdentityServer()
//    .AddAspNetIdentity<ApplicationUser>()
//    .AddProfileService<IdentityServerServiceProvider>();    
    //.AddConfigurationStore(options =>
    //{
    //    options.ConfigureDbContext = builder => builder.UseMySql(Configuration.GetConnectionString("Officer"),
    //        MySqlOptions => MySqlOptions.MigrationAssembly("Damaris.DataService"));
    //});
//builder.Services.AddScoped<IdentityServiceConsumer>();
//builder.Services.AddScoped<IHostedService, IdentityServiceConsumer>();
///builder.Services.AddSingleton<IHostedService, IdentityServiceConsumer>();

//builder.Services.AddScoped<KafkaConsumer<string, string>>();
//builder.Services.AddSingleton<IHostedService, IdentityServiceConsumer>();


//Add background services
//builder.Services.AddSingleton<IHostedService, IdentityServiceConsumer>();

//builder.Services.AddSingleton<IKafkaConsumer<string, string>, KafkaConsumer<string, string>>();
//builder.Services.AddSingleton<IKafkaConsumer>(provider => provider.GetRequiredService<IdentityServiceConsumer>());


//Add MediatR           

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
