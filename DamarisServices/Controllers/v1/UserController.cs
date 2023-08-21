using Confluent.Kafka;
using Damaris.Domain.v1.Models.Account;
using Damaris.Domain.v1.Models.User;
using Damaris.Domain.v2.Models.User;
using DamarisServices.Configurations.Filters;
using DamarisServices.Features.v1.User;
using DamarisServices.Services.v1.User;
using DamarisServices.Utilities.v1;
using KafkaCommunicationLibrary.Producers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

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
        #endregion ======================= [Private Properties] ==============================
        public UserController(IProducer<string, string> producer, ILogger<ApiBaseController> watchdogLogger) : base(producer, watchdogLogger) { }


        
        [HttpPost(Name = "Login")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                //GenerateJwtToken
               var token = await HandleRequestAsync(new CreateTokenRequest() { User = user });
                //i have to generate the request and use 
                //var r = await _kafkaProducer.ProduceAsync("user-logged-in-topic", new Message<string, string>
                //{
                //    Key = user.Id,
                //    Value = "User logged in"
                //});

                return Ok(new { Token = "", UserId = user.Id, UserName = user.UserName });
            }

            return Unauthorized();
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