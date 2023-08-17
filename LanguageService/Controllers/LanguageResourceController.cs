using KafkaCommunicationLibrary.Producers;
using LanguageService.Services;
using Microsoft.AspNetCore.Mvc;

namespace LanguageService.Controllers
{
    [ApiController]
    [Route("api/language")]
    public class LanguageResourceController : ControllerBase
    {                  
        #region Kafka
        private readonly KafkaProducer<string, string> _kafkaProducer;
        #endregion //end kafka prop


        public LanguageResourceController(KafkaProducer<string, string> kafkaproducer)
        {
           _kafkaProducer = kafkaproducer;
        }


        [HttpGet("{culture}")]
        public IActionResult GetLanguageResource(string culture)
        {
            // Implement your logic to fetch language resources
            LanguageResourceService service = new LanguageResourceService();
            var resource = service.GetResource(culture);
            // Serialize resource
            var serializedResource = service.SerializeResource(resource);

            // Publish the serialized resource to Kafka
            _kafkaProducer.Produce("language-resources-topic", culture, serializedResource);

            return Ok(resource);
        }
    }
}
