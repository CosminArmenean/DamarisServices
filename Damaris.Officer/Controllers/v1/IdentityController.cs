using AutoMapper;
using Damaris.Domain.v1.Dtos.Outgoing;
using Damaris.Domain.v1.Dtos.Requests.Account;
using Damaris.Domain.v1.Models.Account;
using Damaris.Domain.v1.Models.User;
using Damaris.Officer.Configuration.Filters;
using Damaris.Officer.Data.v1;
using Damaris.Officer.Domain.v1.Account;
using Damaris.Officer.Features.v1.User;
using Damaris.Officer.Repositories.v1.Interfaces.Generic;
using Damaris.Officer.Utilities.v1;
using Damaris.Officer.Utilities.v1.Generic;
using KafkaCommunicationLibrary.Consumers;
using KafkaCommunicationLibrary.Producers;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
       
        private readonly UserManager<IdentityUser> _userManager;
        private readonly KafkaProducer<string, string> _consumer;
        private readonly KafkaProducer<string, string> _producer;
        private readonly ILogger<IdentityController> _logger;
        private readonly IMapper _mapper;

        public IdentityController(IMediator mediator, KafkaProducer<string, string> producer, KafkaConsumer<string, string> consumer, UserManager<IdentityUser> userManager, ILoggerFactory loggerFactory, IMapper mapper) : base(mediator, producer, consumer, userManager, loggerFactory, mapper) 
        {
            _userManager = userManager;
            _mapper = mapper;
        }



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
        public async Task<IActionResult> RegistrationV1([FromBody] AccountRegistrationRequestDto modelRegistration, [FromServices] IUnitOfWork unitOfWork)
        {
            if(modelRegistration != null)
            {
                IdentityUser user = new IdentityUser();
                IdentityUser jointUser = new IdentityUser();
                if(ModelState.IsValid)
                {
                    if (modelRegistration.Accounts[0].Main == true)
                    {
                        user =  _mapper.Map<IdentityUser>(modelRegistration.Accounts[0]);
                        string username = GenerateUniqueUsername.GenereateUsername(modelRegistration.Accounts[0].FirstName, modelRegistration.Accounts[0].LastName);
                        user.UserName = username;
                        if (modelRegistration.IsJointAccount && modelRegistration.Accounts.Count > 0)
                        {
                            jointUser = _mapper.Map<IdentityUser>(modelRegistration.Accounts[1]);
                            string usernameJoint = GenerateUniqueUsername.GenereateUsername(modelRegistration.Accounts[1].FirstName, modelRegistration.Accounts[1].LastName);
                            jointUser.UserName = usernameJoint;
                        }
                    }
                    if(user != null)
                    {
                        //register user in identity
                        var register = await _userManager.CreateAsync(user);
                        if (register.Succeeded)
                        {
                            return Ok();
                        }
                        else
                        {

                        }
                    }
                               

                    var registration = await HandleRequestAsync(new CreateNewUserRequest() { AccountRegistration = modelRegistration });
                    if(registration != null)
                    {
                        //wait for response
                        //var result = _consumer.
                    }
                }
            }
            
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