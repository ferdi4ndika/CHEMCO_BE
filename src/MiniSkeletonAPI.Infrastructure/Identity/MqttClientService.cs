using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniSkeletonAPI.Application.Common.Interfaces;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;
using MQTTnet.Protocol;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Infrastructure.Identity
{
    public class MqttClientService : IMqttClientService, IDisposable
    {
        private readonly MqttSettings _mqttSettings;
        private readonly IManagedMqttClient _mqttClient;
        private readonly IServiceScopeFactory _scopeFactory;

        public event Action<string> MessageReceived;

        private readonly List<int> _speedBuffer = new();
        private readonly TimeSpan _speedInterval = TimeSpan.FromMinutes(10);
        private DateTime _lastSpeedSaveTime = DateTime.UtcNow;

        private DateTime? _startTime = null;
        private bool _isStarted = false;

        private readonly ConcurrentDictionary<string, object> _latestData = new();

        public MqttClientService(IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _mqttSettings = configuration.GetSection("MqttBrokerSettings").Get<MqttSettings>();
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));

            var factory = new MqttFactory();
            _mqttClient = factory.CreateManagedMqttClient();
        }

        public async Task StartAsync()
        {
            if (_isStarted) return;

            var options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(new MqttClientOptionsBuilder()
                    .WithClientId(_mqttSettings.Id)
                    .WithTcpServer(_mqttSettings.Host, _mqttSettings.Port)
                    //.WithCredentials(_mqttSettings.User, _mqttSettings.Pass)
                    .Build())
                .Build();

            _mqttClient.ApplicationMessageReceivedAsync += HandleReceivedMessage;
            await _mqttClient.StartAsync(options);
            await SubscribeAsync("#");
            _mqttClient.ConnectedAsync += e =>
            {
                Console.WriteLine("✅ Connected ke broker");
                return Task.CompletedTask;
            };

            _mqttClient.DisconnectedAsync += e =>
            {
                Console.WriteLine("❌ Disconnected dari broker");
                return Task.CompletedTask;
            };

            _mqttClient.ConnectingFailedAsync += e =>
            {
                Console.WriteLine($"❌ Gagal connect: {e.Exception}");
                return Task.CompletedTask;
            };

            _mqttClient.ApplicationMessageProcessedAsync += e =>
            {
                Console.WriteLine("📤 Message berhasil diproses dari queue");
                return Task.CompletedTask;
            };
            _mqttClient.ApplicationMessageProcessedAsync += e =>
            {
                Console.WriteLine("📤 TERKIRIM ke broker");
                return Task.CompletedTask;
            };

            _mqttClient.ApplicationMessageSkippedAsync += e =>
            {
                Console.WriteLine("⚠️ Message di-skip");
                return Task.CompletedTask;
            };
            _isStarted = true;
            Console.WriteLine("[MQTT] Client started and subscribed.");
        }

        public async Task PublishAsync<T>(string topic, T payload)
        {
            if (string.IsNullOrWhiteSpace(topic)) throw new ArgumentException("Topic cannot be empty");
            if (payload == null) throw new ArgumentNullException(nameof(payload));

            var json = JsonSerializer.Serialize(payload);
            Console.WriteLine($"IsConnected: {_mqttClient.IsConnected}");
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(Encoding.UTF8.GetBytes(json))
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtMostOnce)
                .Build();

      
            await _mqttClient.EnqueueAsync(message);
            Console.WriteLine("Masuk queue");
        }

        public async Task SubscribeAsync(string topic)
        {
            if (string.IsNullOrWhiteSpace(topic))
                topic = "#";

            var filter = new MqttTopicFilterBuilder()
                .WithTopic(topic)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtMostOnce)
                .Build();

            await _mqttClient.SubscribeAsync(new List<MqttTopicFilter> { filter });
        }

        private async Task HandleReceivedMessage(MqttApplicationMessageReceivedEventArgs e)
        {
            string topic = e.ApplicationMessage.Topic;
            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);

            if (string.IsNullOrWhiteSpace(payload)) return;

            MqttReceivedMessage data = null;
            try
            {
                data = JsonSerializer.Deserialize<MqttReceivedMessage>(payload);
            }
            catch (JsonException)
            {
                if (topic == "conveyor/speed")
                {
                    int speed = int.Parse(payload);
                    await HandleSpeedAsync(speed);
                }
                Console.WriteLine($"Invalid JSON payload: {payload}");
                return;
            }

            if (data != null)
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<IIdentityDataAndonService>();

                if (data.id == "CT")
                {
                    await context.UpdateDetail(CancellationToken.None);
                    Console.WriteLine($"[CT] {topic}: {payload}");
                }
                else if (data.id == "TR")
                {
                    await context.UpdateCounting(CancellationToken.None, false, 0);
                    Console.WriteLine($"[TR] {topic}: {payload}");
                }
                else if (data.id == "STATUS")
                {
                    if (data.value == 0 && _startTime == null)
                        _startTime = DateTime.Now;
                    else if (data.value == 1 && _startTime != null)
                    {
                        TimeSpan duration = DateTime.Now - _startTime.Value;
                        int minutes = BulatkanKeMenitTerdekat(TimeOnly.FromTimeSpan(duration));
                        await context.AddTimeStop(CancellationToken.None, minutes);
                        _startTime = null;
                    }
                }
            }

            MessageReceived?.Invoke(payload);
        }

        private async Task HandleSpeedAsync(int speed)
        {
            lock (_speedBuffer)
            {
                _speedBuffer.Add(speed);

                if (DateTime.UtcNow - _lastSpeedSaveTime >= _speedInterval)
                {
                    int avgSpeed = (int)_speedBuffer.Average();
                    _speedBuffer.Clear();
                    _lastSpeedSaveTime = DateTime.UtcNow;

                    Task.Run(async () =>
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var context = scope.ServiceProvider.GetRequiredService<IIdentityDataAndonService>();
                        await context.UpdateCounting(CancellationToken.None, true, avgSpeed);
                        Console.WriteLine($"Average speed saved: {avgSpeed}");
                    });
                }
            }
        }

        private int BulatkanKeMenitTerdekat(TimeOnly time)
        {
            int menit = time.Minute;
            if (time.Second >= 30) menit++;
            return time.Hour * 60 + menit;
        }

        public void Dispose()
        {
            _mqttClient?.Dispose();
            Console.WriteLine("[MQTT] Client disposed.");
        }

        public class MqttSettings
        {
            public string Id { get; set; }
            public string Host { get; set; }
            public int Port { get; set; }
            public string User { get; set; }
            public string Pass { get; set; }
            public string Topik { get; set; }
        }

        public class MqttReceivedMessage
        {
            public string id { get; set; }
            public int value { get; set; }
        }
    }
}