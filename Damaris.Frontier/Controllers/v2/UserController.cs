using Asp.Versioning;
using Damaris.Frontier.Configurations.Filters;
using KafkaCommunicationLibrary.Producers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Damaris.Frontier.Controllers.v2
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiVersion("2.1")]
    [EnableRateLimiting("ConcurrencyPolicy")]
    [MyActionFilterAttribute("UserController")]
    public class UserController : ControllerBase
    {
       
        private readonly KafkaProducer<string, string> _kafkaService;
        private readonly ILogger<UserController> _logger;

        public UserController(KafkaProducer<string, string> kafkaService, ILogger<UserController> logger)
        {
            _kafkaService = kafkaService;
            _logger = logger;
        }




        [MapToApiVersion("2.0")]
        [HttpGet("profile")]
        //[Authorize] // Requires authentication
        public IActionResult GetUserProfile()
        {
            // Retrieve user profile information from your data source
            var user = new { name = "John Doe2", email = "john.doe@example.com" };

            return Ok(user);
        }

    }
}