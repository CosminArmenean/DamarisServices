using DamarisServices.Configurations.Filters;
using DamarisServices.Services.v1.Health;
using DamarisServices.SupportedCultures.v1;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Versioning;
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
using DamarisServices.Services.v1.User;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(option =>
{
    option.Filters.Add(new IdentityFilter()); //adding global filter
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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add WatchDog Logger
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


builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Configure Redis caching
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
    options.InstanceName = "IdentityMicroservice:";
});

//configuring kafka
var producerConfig = new ProducerConfig();
builder.Configuration.Bind("Kafka:Producer", producerConfig);
builder.Services.AddSingleton(producerConfig);

var consumerConfig = new ConsumerConfig();
builder.Configuration.Bind("Kafka:Consumer", consumerConfig);
builder.Services.AddSingleton(consumerConfig);

builder.Services.AddTransient<IUserService, UserService>();

//===================================================== APP =========================================================
#region APP

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//rate limiter 
app.UseRateLimiter();

//adding localization to enable language selector
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = (IList<CultureInfo>)supportedCultures.Select(c => new CultureInfo(c.CultureCode)),
    SupportedUICultures = (IList<CultureInfo>)supportedCultures.Select(c => new CultureInfo(c.CultureCode))
});

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

//Creating endpoint for my health check
app.MapHealthChecks("/health", new HealthCheckOptions()
{
    Predicate = _=> true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse

});


app.Run();
#endregion //APP