using MQTTnet.Client;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Common.Interfaces
{
    public interface IMqttClientService
    {
        Task PublishAsync(string topic, string payload);

        // Event untuk menangani pesan yang diterima
        event Action<string> MessageReceived;

        // Mengubah return type untuk SubscribeAsync
        Task SubscribeAsync(string topic);
    }
}
