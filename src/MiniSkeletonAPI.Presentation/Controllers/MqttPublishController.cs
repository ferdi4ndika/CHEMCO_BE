//using Microsoft.AspNetCore.Mvc;
//using MQTTnet.Client;
//using MQTTnet;
//using System.Text.Json;
//using MiniSkeletonAPI.Application.Common.Interfaces;
//using MiniSkeletonAPI.Infrastructure.Identity;

//namespace MiniSkeletonAPI.Presentation.Controllers
//{


//    public class MqttPublishRequest
//    {
//        public string? Code { get; set; }
//    }

//    public class MqttPublishDto
//    {
//        public string? Topic { get; set; }
//        public string? Payload { get; set; }
//    }
//    public class MqttPublishController : ControllerBases
//    {
//        private readonly MqttSettings _mqttSettings;
//        private readonly IMqttClientService _mqttClientService;

//        // Inject IConfiguration dan IMqttClientService melalui constructor
//        public MqttPublishController(IConfiguration configuration, IMqttClientService mqttClientService)
//        {
//            _mqttSettings = configuration.GetSection("MqttBrokerSettings").Get<MqttSettings>();
//            _mqttClientService = mqttClientService;
//        }

//        [HttpPost()]

//        public async Task<IActionResult> Publish([FromBody] MqttPublishRequest request)
//        {
//            if (request == null || string.IsNullOrEmpty(request.Code))
//            {
//                return BadRequest("Message cannot be null or empty");
//            }

//            var payload = new { id = request.Code };
//            var message = JsonSerializer.Serialize(payload);

//            try
//            {
//                await _mqttClientService.PublishAsync(_mqttSettings.Topik, message);
//                return Ok(new { Message = "Published successfully" });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Error while publishing: {ex.Message}");
//            }
//        }

//        [HttpPost("code")]
//        public async Task<IActionResult> Publish2([FromBody] MqttPublishRequest request)
//        {
//            if (request == null || string.IsNullOrEmpty(request.Code))
//            {
//                return BadRequest("Message cannot be null or empty");
//            }

//            var payload = new { code = request.Code };

//            var message = JsonSerializer.Serialize(payload);

//            try
//            {
//                await _mqttClientService.PublishAsync(_mqttSettings.Topik, message);
//                return Ok(new { Message = "Published successfully" });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Error while publishing: {ex.Message}");
//            }
//        }

//        [HttpPost("publish-message")]
//        public async Task<IActionResult> PublishMessage(MqttPublishDto publishDto)
//        {
//            if (string.IsNullOrEmpty(publishDto.Topic) || string.IsNullOrEmpty(publishDto.Payload))
//            {
//                return BadRequest("Topic and payload are required");
//            }

//            try
//            {
//                await _mqttClientService.PublishAsync(publishDto.Topic, publishDto.Payload);
//                return Ok(new { message = "Message published successfully" });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Internal server error: {ex.Message}");
//            }
//        }
//    }




//}
