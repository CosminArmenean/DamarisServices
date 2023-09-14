using AutoMapper;
using Confluent.Kafka;
using Damaris.DataService.Repositories.v1.Interfaces.Generic;
using Damaris.DataService.Utilities.v1;
using Damaris.Domain.v1.Dtos.GenericDtos;
using KafkaCommunicationLibrary.Consumers;
using KafkaCommunicationLibrary.Producers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Damaris.DataService.Controllers.v1
{
    public class IdentityController : ApiBaseController
    {
        private readonly IProducer<string, string> _producer;
        private readonly IConsumer<string, string> _consumer;
        private readonly ILogger<ApiBaseController> _watchLogger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        public IdentityController(ILoggerFactory loggerFactory, IMediator mediator, IUnitOfWork unitOfWork, IMapper mapper, KafkaProducer<string, string> producer, KafkaConsumer<string, string> consumer) : base(loggerFactory, mediator, unitOfWork, mapper, producer, consumer)
        {        
        }

        [HttpPost("/process-register")]
        public async Task<IActionResult> ProcessRegisterAsync()
        {
            try
            {
                // Create a ConsumerRecord object
                //var consumerRecord = new ConsumerRecord<string, string>("topic", 0, 0, "dummy data");

                // Consume the message from Kafka
                // var task = await _consumer.ConsumeAsync(consumerRecord);

                //// Do something with the message
                //if (task.IsCompletedSuccessfully)
                //{
                //    // The message was consumed successfully
                //}
                //else
                //{
                //    // The message was not consumed successfully
                //}
                // Process the register request from Kafka
                //var record = await _consumer.ConsumeAsync();

                //var userId = record.Key;
                //var password = record.Value;

                // Do something with the register request
                // ...
                return await  Task.FromResult( Ok());
            }
            catch (Exception ex)
            {
                // Handle the exception
                _watchLogger.LogError(ex, "Failed to process register request");
                return await Task.FromResult(Ok());
            }
        }
    }
}
