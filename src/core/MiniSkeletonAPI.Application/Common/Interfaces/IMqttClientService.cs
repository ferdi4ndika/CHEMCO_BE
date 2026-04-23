using System;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Common.Interfaces
{
    public interface IMqttClientService
    {
        /// <summary>
        /// Start MQTT client dan subscribe topik default
        /// </summary>
        Task StartAsync();

        /// <summary>
        /// Publish payload ke topic tertentu
        /// </summary>
        Task PublishAsync<T>(string topic, T payload);

        /// <summary>
        /// Subscribe ke topic tertentu
        /// </summary>
        Task SubscribeAsync(string topic);

        /// <summary>
        /// Event yang dipanggil saat pesan MQTT diterima
        /// </summary>
        event Action<string> MessageReceived;
    }
}