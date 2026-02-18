
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MiniSkeletonAPI.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;


namespace MiniSkeletonAPI.Presentation.Controllers;
public class MqttBackgroundService : BackgroundService
{
    private readonly IMqttClientService _mqttService;
    private readonly ILogger<MqttBackgroundService> _logger;

    public MqttBackgroundService(
        IMqttClientService mqttService,
        ILogger<MqttBackgroundService> logger)
    {
        _mqttService = mqttService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("🚀 MQTT Background Service started");

      
        await _mqttService.SubscribeAsync("#");

        _mqttService.MessageReceived += payload =>
        {
            _logger.LogInformation($"📩 MQTT Message: {payload}");

            // di sini MQTT kamu BENAR-BENAR jalan
            // - simpan DB
            // - kirim SignalR
            // - logic lain
        };

        // biar service tetap hidup
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}

