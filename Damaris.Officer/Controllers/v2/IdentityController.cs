using AutoMapper;
using Damaris.Domain.v1.Dtos.Requests.Account;
using Damaris.Domain.v1.Models.User;
using Damaris.Officer.Configuration.Filters;
using Damaris.Officer.Utilities.v1;
using KafkaCommunicationLibrary.Consumers;
using KafkaCommunicationLibrary.Producers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Damaris.Officer.Controllers.v2
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiVersion("2.1")]
    [EnableRateLimiting("ConcurrencyPolicy")]
    [MyActionFilterAttribute("IdentityUserFilterAttribute")]
    public class IdentityController : ApiBaseController
    {


        private readonly ILogger<IdentityController> _logger;
        private readonly IMapper _mapper;

        public IdentityController(IMediator mediator, KafkaProducer<string, string> producer, KafkaConsumer<string, string> consumer, ILoggerFactory loggerFactory, IMapper mapper) : base(mediator, producer, consumer, loggerFactory, mapper) { }


        [HttpGet(Name = "GetWeatherForecast")]
        [MapToApiVersion("2.0")]
        [Route("Get")]
        public IEnumerable<WeatherForecast> GetV2()
        {
            string[] Summaries = new[]
            {
                     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
             };
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [Route("Login")]
        [HttpPost(Name = "Login")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> LoginV2([FromBody] LoginRequest model)
        {
            //ApplicationUser user = new() { UserName = "Cosmin", PasswordHash = "test" };
            //user = await _userManager.FindByNameAsync(model.UserName);
            ////if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))

            //{
            //GenerateJwtToken
            //var login = await HandleRequestAsync(new CreateLoginRequest() { LoginRequest = model });
            // Wait for response from Kafka topic


            // Return processed data as response to the user
            return Ok();
            return Unauthorized();
        }

        [Route("Register")]
        [HttpPost(Name = "Register")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> RegisterV2([FromBody] AccountRegistrationRequestDto model)
        {
            //ApplicationUser user = new() { UserName = "Cosmin", PasswordHash = "test" };
            //user = await _userManager.FindByNameAsync(model.UserName);
            ////if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))

            //{
            //GenerateJwtToken
            //var login = await HandleRequestAsync(new CreateLoginRequest() { LoginRequest = model });
            // Wait for response from Kafka topic


            // Return processed data as response to the user
            return Ok();
            return Unauthorized();
        }
    }
}
