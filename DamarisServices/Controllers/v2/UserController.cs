using DamarisServices.Configurations.Filters;
using KafkaCommunicationLibrary.Producers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace DamarisServices.Controllers.v2
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiVersion("2.1")]
    [EnableRateLimiting("ConcurrencyPolicy")]
    [MyActionFilterAttribute("UserController")]
    public class UserController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Hot hot hot hot hot", "cold cold cold"
    };
        private readonly KafkaProducer<string, string> _kafkaService;
        private readonly ILogger<UserController> _logger;

        public UserController(KafkaProducer<string, string> kafkaService, ILogger<UserController> logger)
        {
            _kafkaService = kafkaService;
            _logger = logger;
        }


        
        [HttpPost(Name = "LoginV2")]
        [MapToApiVersion("2.0")]
        public IActionResult Login()
        {
            // Authenticate user and generate token

            // Publish user authentication event to Kafka
            _kafkaService.Produce("user-authentication-topic", "userLoggedInEvent", "User logged in");

            return Ok(new { Token = "your_generated_token" });
        }

        
        
    }
}