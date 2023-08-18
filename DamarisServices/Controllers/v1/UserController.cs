using Confluent.Kafka;
using Damaris.DataService.Utilities.v1;
using Damaris.Domain.v1.Models.User;
using Damaris.Domain.v2.Models.User;
using DamarisServices.Features.v1.User;
using DamarisServices.Services.v1.User;
using KafkaCommunicationLibrary.Producers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DamarisServices.Controllers.v1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0", Deprecated = true)]
    public class UserController : ApiBaseController
    {
        #region ======================= [Private Properties] ==============================

        private readonly UserManager<User> _userManager;
        #endregion ======================= [Private Properties] ==============================
        public UserController(IProducer<string, string> producer, IConsumer<string, string> consumer, ILogger<ApiBaseController> watchdogLogger) : base(producer, consumer, watchdogLogger) { }
        


        [HttpPost(Name = "Login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                //var token = GenerateJwtToken(user);
                //i have to generate the request and use 
                //await _kafkaProducer.ProduceAsync("user-logged-in-topic", new Message<string, string>
                //{
                //    Key = user.Id,
                //    Value = "User logged in"
                //});

                return Ok(new { Token = "", UserId = user.Id, UserName = user.UserName });
            }

            return Unauthorized();
        }

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