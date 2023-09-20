using Confluent.Kafka;
using Damaris.Domain.v1.Dtos.Requests.Account;
using Damaris.Domain.v1.Models.Account;
using Damaris.Domain.v1.Models.User;
using Damaris.Domain.v2.Models.User;
using DamarisServices.Configurations.Filters;
using DamarisServices.Features.v1.User;
using DamarisServices.Services.v1.User;
using DamarisServices.Utilities.v1;
using DamarisServices.Utilities.v1.Generic;
using KafkaCommunicationLibrary.Consumers;
using KafkaCommunicationLibrary.Producers;
using KafkaCommunicationLibrary.Repositories.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace DamarisServices.Controllers.v1
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0", Deprecated = false)]
    [ApiVersion("1.1")]
    [EnableRateLimiting("ConcurrencyPolicy")]
    [MyActionFilterAttribute("UserController")]
    public class UserController : ApiBaseController
    {
        #region ======================= [Private Properties] ==============================

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProducer<string, string> _producer;
        private readonly KafkaConsumer<string, string> _consumer;
        #endregion ======================= [Private Properties] ==============================
        public UserController(IMediator mediator, KafkaProducer<string, string> producer, KafkaConsumer<string, string> consumer, ILoggerFactory loggerFactory) : base(mediator, producer, consumer, loggerFactory) { }

        [HttpPost(Name = "Register")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Register([FromBody] AccountRegistrationRequestDto model)
        {
            var registration = await HandleRequestAsync(new CreateNewUserRequest() { AccountRegistration = model });

            return Ok();
        }

        
        [HttpPost(Name = "Login")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            ApplicationUser user = new() {   UserName = "Cosmin", PasswordHash = "test"};
            //user = await _userManager.FindByNameAsync(model.UserName);
            ////if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            
            //{
            //GenerateJwtToken
               var login = await HandleRequestAsync(new CreateLoginRequest() {  LoginRequest = model });
            // Wait for response from Kafka topic
            

            // Return processed data as response to the user
            return Ok();

            //i have to generate the request and use 
            //var r = await _kafkaProducer.ProduceAsync("user-logged-in-topic", new Message<string, string>
            //{
            //    Key = user.Id,
            //    Value = "User logged in"
            //});

            // return Ok(new { Token = "", UserId = user.Id, UserName = user.UserName });
            //}

            return Unauthorized();
        }

        [MapToApiVersion("1.0")]
        [HttpGet("profile")]
        //[Authorize] // Requires authentication
        public IActionResult GetUserProfile()
        {
            // Retrieve user profile information from your data source
            var user = new { name = "John Doe", email = "john.doe@example.com" };

            return Ok(user);
        }

        //// [Route("api/account/logout")]
        //[HttpPost(Name = "Logout")]       
        //[MapToApiVersion("1.0")]
        //public async Task<IActionResult> Logout([FromBody] LoginRequest model)
        //{
        //    // _tokenManager.Invalidate(User.Identity.Name);
        //    ApplicationUser user = await _userManager.FindByNameAsync(model.UserName);
        //    if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        //    {
        //        return Ok();
        //    }
        //    return Ok();
        //}


        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] RegisterUser model)
        //{
        //    var user = new User
        //    {
        //        MobilePhone = model.MobilePhone,
        //        Email = model.Email   
        //    };

        //    //implement here 
        //    var result = await HandleRequestAsync(new CreateNewUserRequest() {  UserId = "test", Payload = new Damaris.Domain.v1.Dtos.GenericDtos.ProducerRecord() { Value = "test" } });


        //    var result = await _userManager.CreateAsync(user, model.Password);
        //    if (result.Succeeded)
        //    {
        //        // User registered successfully
        //        await _kafkaProducer.ProduceAsync("user-registered-topic", new Message<string, string>
        //        {
        //            Key = user.Id,
        //            Value = "New user registered"
        //        });

        //        return Ok(new { message = "User registered successfully" });
        //    }

        //    // User registration failed
        //    return BadRequest(result.Errors);
        //}


    }
}