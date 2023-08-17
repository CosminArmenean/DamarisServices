using DamarisServices.Services.v1.User;
using KafkaCommunicationLibrary.Producers;
using Microsoft.AspNetCore.Mvc;

namespace DamarisServices.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0", Deprecated = true)]
    public class UserController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
        private readonly KafkaProducer<string, string> _kafkaService;
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(KafkaProducer<string, string> kafkaService, ILogger<UserController> logger)
        {
            _kafkaService = kafkaService;
            _logger = logger;
        }


        [HttpPost(Name = "Login")]       
        public IActionResult Login()
        {
            // Authenticate user
            var user = _userService.Authenticate(model.Username, model.Password);
            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid credentials" });
            }

            // Generate token and send user authentication event to Kafka
            var token = _userService.GenerateToken(user);
            _kafkaService.Produce("user-authentication-topic", user.Id.ToString(), "User logged in");

            return Ok(new { Token = token });
        }

        [HttpGet(Name = "GetWeatherForecast")]
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
    }
}