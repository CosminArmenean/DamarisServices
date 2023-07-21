using DamarisServices.Services.Health;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Versioning;
using WatchDog;
using WatchDog.src.Enums;
using WatchDog.src.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});


// This will tell ASP.NET Core how to determine the user's preferred language
// and how to return localized strings.
builder.Services.UseLocalization(options =>
{
    options. = "en-US";
    options.SupportedCultures = new[] { "en-US", "es-ES" };
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
