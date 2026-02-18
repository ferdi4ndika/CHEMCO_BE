using System.Threading.Tasks;
using MiniSkeletonAPI.Application.Common.Interfaces;

namespace MiniSkeletonAPI.Presentation.Services
{
    public class MqttSevis
    {
        private readonly IMqttClientService _mqttClientService;

        public MqttSevis(IMqttClientService mqttClientService)
        {
            _mqttClientService = mqttClientService;
        }

        public async Task SendMessageAsync()
        {
            string topic = "your/topic";
            string payload = "Your message here";

            await _mqttClientService.PublishAsync(topic, payload);
        }

        public async Task SubscribeToTopicAsync()
        {
            string topic = "makan";
            await _mqttClientService.SubscribeAsync(topic);
        }
    }
}
