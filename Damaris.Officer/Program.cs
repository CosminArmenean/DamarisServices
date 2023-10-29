using Confluent.Kafka;
using Damaris.Common.v1.SupportedCultures;
using Damaris.Officer.Repositories.v1.Implementations.TopicEventsProcessor;
using KafkaCommunicationLibrary.Consumers;
using KafkaCommunicationLibrary.Domain.Models;
using KafkaCommunicationLibrary.Producers;
using KafkaCommunicationLibrary.Repositories.Interfaces;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Collections.Concurrent;
using System.Globalization;
using WatchDog;
using WatchDog.src.Enums;
using Damaris.Officer.Services.v1;
using System.Reflection;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Damaris.Officer.Repositories.v1.Interfaces.UserInterface;
using Damaris.Officer.Repositories.v1.Implementations.UserImplementation;
using Damaris.Officer.Repositories.v1.Interfaces.Generic;
using Damaris.Officer.Repositories.v1;
using Damaris.Officer.Data.v1;
using Microsoft.EntityFrameworkCore;
using Damaris.Officer.Configuration.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var officerMysql = builder.Configuration.GetConnectionString(name: "Officer");

builder.Services.AddControllers(option =>
{
    option.Filters.Add(new IdentityFilter());//global filter
});


builder.Services.AddMvc();
builder.Services.AddSwaggerGen( c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Identity API", Version = "v1" });
    c.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Identity API", Version = "v2" });
    c.EnableAnnotations();
    c.ResolveConflictingActions(apiDescription => apiDescription.First());

});
builder.Services.AddSwaggerGenNewtonsoftSupport();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
//});
//Add api versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        //new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("x-version")
    //new MediaTypeApiVersionReader("ver")
    );
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


//Initialize AutoMapper
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddLogging(logging => logging.AddWatchDogLogger());

builder.Services.AddWatchDogServices(opt =>
{
    opt.IsAutoClear = false;
    //opt.ClearTimeSchedule = WatchDogAutoClearScheduleEnum.Monthly;

    opt.SetExternalDbConnString = builder.Configuration.GetConnectionString(name: "WatchDogPostGreSql");
    opt.DbDriverOption = WatchDogDbDriverEnum.PostgreSql;
});

//Initializing my DbContext inside the DI Container
builder.Services.AddDbContext<OfficerDbContext>(options =>
{
    options.UseMySql(officerMysql, ServerVersion.AutoDetect(officerMysql));
}, ServiceLifetime.Singleton);

//Configure Supported Cultures - Languages
// Configure supported cultures and localization options
var cultureProvider = new SupportedCultureProvider();
var supportedCultures = cultureProvider.GetSupportedCultures();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-US");
    options.SupportedCultures = (IList<CultureInfo>?)supportedCultures.Select(c => new CultureInfo(c.CultureCode));
    options.SupportedUICultures = (IList<CultureInfo>?)supportedCultures.Select(c => new CultureInfo(c.CultureCode));


});
builder.Services.AddLocalization();

//Add api versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.ReportApiVersions = true;    
    options.ApiVersionReader = ApiVersionReader.Combine(
        //new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("x-version")
        //new MediaTypeApiVersionReader("ver")
    );
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

//Add Rate Limiter fixed window  x/user
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("FixedWindowpolicy", opt =>
    {
        opt.Window = TimeSpan.FromSeconds(5);
        opt.PermitLimit = 5;
        opt.QueueLimit = 5;
        opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    }).RejectionStatusCode = 429; // To many requests
});

//Add Concurency Rate Limiter 
builder.Services.AddRateLimiter(options =>
{
    options.AddConcurrencyLimiter("ConcurrencyPolicy", opt =>
    {
        opt.PermitLimit = 5;
        opt.QueueLimit = 5;
        opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    }).RejectionStatusCode = 429; // To many requests
});

//Add Token Bucket  Rate Limiter 
builder.Services.AddRateLimiter(options =>
{
    options.AddTokenBucketLimiter("TokenBucketPolicy", opt =>
    {
        opt.TokenLimit = 1000;
        opt.QueueLimit = 500;
        opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        opt.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
        opt.TokensPerPeriod = 1000;
        opt.AutoReplenishment = true;
    }).RejectionStatusCode = 429; // To many requests
});




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
    Partitioner = Confluent.Kafka.Partitioner.Consistent

    // Other producer configuration options
};
builder.Services.AddSingleton(producerConfig);

builder.Services.AddSingleton<KafkaConsumer<string, string>>();
builder.Services.AddSingleton<KafkaProducer<string, string>>();

builder.Services.AddSingleton<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();

//Register the topic event processors as scoped services:
builder.Services.AddSingleton<IKafkaTopicEventProcessor<string, string>, RegisterUserEventProcessor>();


builder.Services.AddHostedService<OfficerConsumerService>();
builder.Services.AddSingleton<OfficerConsumerService>();

//adding cors 
var OfficerAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: OfficerAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
        });
});

//Adding IdentityServer4-net6
builder.Services.AddIdentityServer()
    .AddInMemoryClients(new List<Client>())
    .AddInMemoryIdentityResources(new List<IdentityResource>())
    .AddInMemoryApiResources(new List<ApiResource>())
    .AddInMemoryApiScopes(new List<ApiScope>())
    .AddTestUsers(new List<TestUser>())
    .AddDeveloperSigningCredential();

//Add MediatR           
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI( c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity Server API V1");
    });
}

//rate limiter 
app.UseRateLimiter();

//adding localization to enable language selector
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = (IList<CultureInfo>)supportedCultures.Select(c => new CultureInfo(c.CultureCode)).ToList(),
    SupportedUICultures = (IList<CultureInfo>)supportedCultures.Select(c => new CultureInfo(c.CultureCode)).ToList()
});

app.UseHttpsRedirection();

app.UseCors(OfficerAllowSpecificOrigins);

app.UseAuthorization();

//inject WatchDog Logger to the middleware 
app.UseWatchDogExceptionLogger();
//This authentication information (Username and Password) will be used to access the log viewer.
app.UseWatchDog(opt =>
{
    opt.WatchPageUsername = "admin";
    opt.WatchPagePassword = "Qwerty@123";
});

app.UseIdentityServer();

app.MapControllers();

app.Run();
