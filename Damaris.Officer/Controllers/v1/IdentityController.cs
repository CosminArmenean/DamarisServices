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
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using KafkaCommunicationLibrary.Consumers;
using KafkaCommunicationLibrary.Producers;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Annotations;
using Damaris.Officer.Utilities.v1.Extensions;

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
        private readonly IEventService _events;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IClientStore _clientStore;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly SignInManager<IdentityUser> _signInManager;

        public IdentityController(
            IMediator mediator,           
            UserManager<IdentityUser> userManager,  
            ILoggerFactory loggerFactory, 
            IMapper mapper,
            SignInManager<IdentityUser> signInManager,
            IEventService events,
            IAuthenticationSchemeProvider schemeProvider,
            IClientStore clientStore,
            IIdentityServerInteractionService interaction) : 
            base(
                mediator,             
                userManager, 
                loggerFactory, 
                mapper) 
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<IdentityController>();
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
        }

        



        //[Route("Login")]
        [MapToApiVersion("1.1")]
        [MapToApiVersion("1.0")]
        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginV1([FromBody] LoginRequest modelLogin)
        {

            // check if we are in the context of an authorization request
            var context = await _interaction.GetAuthorizationContextAsync(modelLogin.ReturnUrl);
            
            if (ModelState.IsValid)
            {
                var user = await _signInManager.UserManager.FindByNameAsync(modelLogin.Username);
                if(user != null)
                {
                    var userLogin = await _signInManager.CheckPasswordSignInAsync(user, modelLogin.Password, true);
                    // validate username/password against in-memory store
                    if (userLogin == Microsoft.AspNetCore.Identity.SignInResult.Success)
                    {
                        
                        await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId));

                        // only set explicit expiration here if user chooses "remember me". 
                        // otherwise we rely upon expiration configured in cookie middleware.
                        AuthenticationProperties props = null;
                        if (AccountOptions.AllowRememberLogin && modelLogin.RememberLogin)
                        {
                            props = new AuthenticationProperties
                            {
                                IsPersistent = true,
                                ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
                            };
                        };

                        // issue authentication cookie with subject ID and username
                        var isuser = new IdentityServerUser(user.Id)
                        {
                            DisplayName = user.UserName
                        };

                        await HttpContext.SignInAsync(isuser, props);

                        if (context != null)
                        {
                            if (context.IsNativeClient())
                            {                                
                                // The client is native, so this change in how to
                                // return the response is for better UX for the end user.
                                return this.LoadingPage("Redirect", modelLogin.ReturnUrl);
                            }

                            // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                            return Redirect(modelLogin.ReturnUrl);
                        }

                        // request for a local page
                        if (Url.IsLocalUrl(modelLogin.ReturnUrl))
                        {
                            return Redirect(modelLogin.ReturnUrl);
                        }
                        else if (string.IsNullOrEmpty(modelLogin.ReturnUrl))
                        {
                            return Redirect("~/");
                        }
                        else
                        {
                            // user might have clicked on a malicious link - should be logged
                            throw new Exception("invalid return URL");
                        }
                    }

                }               

                await _events.RaiseAsync(new UserLoginFailureEvent(modelLogin.Username, "invalid credentials", clientId: context?.Client.ClientId));
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
            }

            // something went wrong, show form with error
           return BadRequest();

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