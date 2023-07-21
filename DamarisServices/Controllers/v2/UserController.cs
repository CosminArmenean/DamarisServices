using Microsoft.AspNetCore.Mvc;

namespace DamarisServices.Controllers.v2
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiVersion("2.1")]
    public class UserController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Hot hot hot hot hot", "cold cold cold"
    };

        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecastV2")]
        [MapToApiVersion("2.0")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [HttpGet(Name = "GetWeatherForecastV21")]
        [MapToApiVersion("2.1")]
        public IEnumerable<WeatherForecast> GetV1()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = "Wheather is hot!! - v2.1"
            })
            .ToArray();
        }
    }
}