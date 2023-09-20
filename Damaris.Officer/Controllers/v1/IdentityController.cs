using Damaris.Domain.v1.Dtos.Requests.Account;
using Damaris.Domain.v1.Models.Account;
using Damaris.Domain.v1.Models.User;
using Damaris.Officer.Configuration.Filters;
using Damaris.Officer.Utilities.v1;
using KafkaCommunicationLibrary.Consumers;
using KafkaCommunicationLibrary.Producers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;

namespace Damaris.Officer.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0", Deprecated = false)]    
    [EnableRateLimiting("ConcurrencyPolicy")]
    [MyActionFilterAttribute("IdentityUserFilterAttribute")]    
    public class IdentityController : ApiBaseController
    {
        

        private readonly ILogger<IdentityController> _logger;

        public IdentityController(IMediator mediator, KafkaProducer<string, string> producer, KafkaConsumer<string, string> consumer, ILoggerFactory loggerFactory) : base(mediator, producer, consumer, loggerFactory) { }



        //[Route("Login")]
        [MapToApiVersion("1.1")]
        [MapToApiVersion("1.0")]
        [HttpPost("Login")]
        public async Task<IActionResult> LoginV1([FromBody] LoginRequest modelLogin)
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
            
        }

        [HttpPost("Registration")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> RegistrationV1([FromBody] AccountDto modelRegistration)
        {
            return Ok();
        }
        //[Route("Register")]
        //[MapToApiVersion("1.0")]
        //[HttpPost("Register")]
        //public async Task<IActionResult> RegisterV1([FromBody] AccountRegistrationRequestDto model)
        //{
        //    //ApplicationUser user = new() { UserName = "Cosmin", PasswordHash = "test" };
        //    //user = await _userManager.FindByNameAsync(model.UserName);
        //    ////if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))

        //    //{
        //    //GenerateJwtToken
        //    //var login = await HandleRequestAsync(new CreateLoginRequest() { LoginRequest = model });
        //    // Wait for response from Kafka topic


        //    // Return processed data as response to the user
        //    return Ok();
          
        //}
    }
}