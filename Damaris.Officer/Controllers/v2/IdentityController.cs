using Asp.Versioning;
using AutoMapper;
using Damaris.Domain.v1.Dtos.Requests.Account;
using Damaris.Domain.v1.Models.User;
using Damaris.Officer.Configuration.Filters;
using Damaris.Officer.Utilities.v1;
using KafkaCommunicationLibrary.Consumers;
using KafkaCommunicationLibrary.Producers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Damaris.Officer.Controllers.v2
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")] 
    [EnableRateLimiting("ConcurrencyPolicy")]
    [MyActionFilterAttribute("IdentityUserFilterAttribute")]
    [ApiVersion("2.0")]
    public class IdentityController : ApiBaseController
    {


        private readonly ILogger<IdentityController> _logger;
        private readonly IMapper _mapper;

        public IdentityController(IMediator mediator, UserManager<IdentityUser> userManager, ILoggerFactory loggerFactory, IMapper mapper) : base(mediator, userManager, loggerFactory, mapper) { }




        [MapToApiVersion("2.0")]
        [Route("Login")]
        [HttpPost(Name = "Login")]      
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
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

        [MapToApiVersion("2.0")]
        [Route("Register")]
        [HttpPost(Name = "Register")]      
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
