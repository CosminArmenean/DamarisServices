using Damaris.Frontier.Configurations.Filters;
using Damaris.Frontier.Services.v1.Health;
using Damaris.Frontier.SupportedCultures.v1;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Globalization;
using WatchDog;
using WatchDog.src.Enums;
using WatchDog.src.Models;
using StackExchange.Redis;
using Confluent.Kafka;
using Damaris.Frontier.Services.v1.User;
using Microsoft.EntityFrameworkCore;
using KafkaCommunicationLibrary.Consumers;
using KafkaCommunicationLibrary.Producers;
using Damaris.Frontier.Repositories.v1.Implementation.TopicEventsProcessor;
using KafkaCommunicationLibrary.Repositories.Interfaces;
using System.Reflection;
using Microsoft.Extensions.Logging;
using KafkaCommunicationLibrary.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Asp.Versioning;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Damaris.Frontier.Configurations.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(option =>
{
    option.Filters.Add(new IdentityFilter()); //adding global filter
});

//adding bearer for IdentityServer4
builder.Services.AddAuthentication("Bearer")
    .AddIdentityServerAuthentication("Bearer", options =>
    {
        options.Authority = "https://localhost:44383";
        options.ApiName = "innkt";
    });

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



builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        //new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("x-version")
    //new MediaTypeApiVersionReader("ver")
    );
})
.AddApiExplorer(options =>
{
    // Add the versioned API explorer, which also adds IApiVersionDescriptionProvider service
    // note: the specified format code will format the version as "'v'major[.minor][-status]"
    options.GroupNameFormat = "'v'VVV";

    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
    // can also be used to control the format of the API version in route templates
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(options =>
{
    // Add a custom operation filter which sets default values
    options.OperationFilter<SwaggerDefaultValues>();
});




//Add WatchDog Logger
builder.Services.AddLogging(logging => logging.AddWatchDogLogger());

builder.Services.AddWatchDogServices(opt =>
{
    opt.IsAutoClear = false;
    //opt.ClearTimeSchedule = WatchDogAutoClearScheduleEnum.Monthly;
    
    opt.SetExternalDbConnString = builder.Configuration.GetConnectionString(name: "PostGreSqlConnection");
    opt.DbDriverOption = WatchDogDbDriverEnum.PostgreSql;
});


//Add health checks
builder.Services.AddHealthChecks()
    .AddCheck<ApiHealthCheck>("TestApi", tags: new string[] {"Aiport API"});
    


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




// Configure Redis caching
//builder.Services.AddStackExchangeRedisCache(options =>
//{
//    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
//    options.InstanceName = "IdentityMicroservice:";
//});


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

builder.Services.AddScoped<KafkaConsumer<string, string>>();
builder.Services.AddScoped<KafkaProducer<string, string>>();

//Register the topic event processors as scoped services:
builder.Services.AddScoped<IKafkaTopicEventProcessor<string, string>, LoginEventProcessor>();


//Add MediatR           
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));


// Configure JWT authentication
//need to proper configur this
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = "your-issuer",
//            ValidAudience = "your-audience",
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secret-key"))
//        };
//    });

//adding cors 
var frontierAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: frontierAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
        });
});



//===================================================== APP =========================================================
#region APP

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();

        // Build a swagger endpoint for each discovered API version
        foreach (var description in descriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
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
app.UseCors(frontierAllowSpecificOrigins);

app.UseRouting();


app.UseAuthentication();


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

//Creating endpoint for my health check
app.MapHealthChecks("/health", new HealthCheckOptions()
{
    Predicate = _=> true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse

});


app.Run();
#endregion //APP