using Asp.Versioning;
using Confluent.Kafka;
using Damaris.Domain.v1.Dtos.Requests.Account;
using Damaris.Domain.v1.Models.Account;
using Damaris.Domain.v1.Models.User;
using Damaris.Domain.v2.Models.User;
using Damaris.Frontier.Configurations.Filters;
using Damaris.Frontier.Features.v1.User;
using Damaris.Frontier.Services.v1.User;
using Damaris.Frontier.Utilities.v1;
using Damaris.Frontier.Utilities.v1.Generic;
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

namespace Damaris.Frontier.Controllers.v1
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0", Deprecated = false)]
    [ApiVersion("1.1")]
    [EnableRateLimiting("ConcurrencyPolicy")]
    [MyActionFilterAttribute("UserController")]
    [Authorize]
    public class UserController : ApiBaseController
    {
        #region ======================= [Private Properties] ==============================

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProducer<string, string> _producer;
        private readonly KafkaConsumer<string, string> _consumer;
        #endregion ======================= [Private Properties] ==============================
        public UserController(IMediator mediator, KafkaProducer<string, string> producer, KafkaConsumer<string, string> consumer, ILoggerFactory loggerFactory) : base(mediator, producer, consumer, loggerFactory) { }

        

       

        [MapToApiVersion("1.0")]
        [HttpGet("profile")]
        //[Authorize] // Requires authentication
        public IActionResult GetUserProfile()
        {
            // Retrieve user profile information from your data source
            var user = new { name = "John Doe", email = "john.doe@example.com" };

            return Ok(user);
        }

       

    }
}