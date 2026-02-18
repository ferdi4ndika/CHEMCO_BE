using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniSkeletonAPI.Application.Common.Interfaces;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Infrastructure.Identity
{
    public class MqttClientService : IMqttClientService
    {
        private readonly MqttSettings _mqttSettings;
        private readonly IManagedMqttClient _mqttClient;
        private readonly IServiceScopeFactory _scopeFactory;

        // Event untuk notify listener lain
        public event Action<string> MessageReceived;

        private  DateTime? _startTime = null;

        public MqttClientService(
            IConfiguration configuration,
            IServiceScopeFactory scopeFactory)
        {
            _mqttSettings = configuration.GetSection("MqttBrokerSettings").Get<MqttSettings>();
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));

            var mqttFactory = new MqttFactory();
            _mqttClient = mqttFactory.CreateManagedMqttClient();

            var options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(new MqttClientOptionsBuilder()
                .WithClientId(_mqttSettings.Id)
                    .WithTcpServer(_mqttSettings.Host, _mqttSettings.Port)
                    .WithCredentials(_mqttSettings.User,_mqttSettings.Pass)
                    .Build())
                .Build();

            _mqttClient.StartAsync(options).GetAwaiter().GetResult();
            _mqttClient.ApplicationMessageReceivedAsync += HandleReceivedMessage;
        }

        // Publish ke topic MQTT
        public async Task PublishAsync(string topic, string payload)
        {
            if (string.IsNullOrEmpty(topic) || string.IsNullOrEmpty(payload))
                throw new ArgumentException("Topic and payload cannot be null");

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce)
                .Build();

            await _mqttClient.EnqueueAsync(message);
        }

        // Subscribe ke topic
        public async Task SubscribeAsync(string topic)
        {
            if (string.IsNullOrEmpty(topic))
                topic = "#"; // subscribe semua topic

            var topicFilter = new MqttTopicFilterBuilder()
                .WithTopic(topic)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce)
                .Build();

            await _mqttClient.SubscribeAsync(new List<MqttTopicFilter> { topicFilter });
        }

        // Handle pesan masuk
        private async Task HandleReceivedMessage(MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                string topic = e.ApplicationMessage.Topic;
                string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                using var scope = _scopeFactory.CreateScope();
                var contex = scope.ServiceProvider.GetRequiredService<IIdentityDataAndonService>();
                if (string.IsNullOrWhiteSpace(payload))
                    return;

                MqttReceivedMessage data = null;
                try
                {
                    data = JsonSerializer.Deserialize<MqttReceivedMessage>(payload);
                }
                catch (JsonException)
                {
                    if(topic == "conveyor/speed")
                    {
                        int da = int.Parse(payload);
                        await contex.UpdateCounting(CancellationToken.None, true, da);
                        //Console.WriteLine($"Invalid JSON payload: {payload}");
                        //return;

                    }

                    Console.WriteLine($"Invalid JSON payload: {payload}");
                    return;
                }

                if (data != null)
                {
                    // Gunakan scope untuk scoped service

                    Console.WriteLine("data MQtt", data.ToString() );
                    if (data.id == "CT")
                        await contex.UpdateDetail(CancellationToken.None);
                    else if (data.id == "TR")
                        await contex.UpdateCounting(CancellationToken.None,false, 0 );
                   if (data.id == "STATUS")
                    {
                        if(data.value == 0 && _startTime == null)
                        {
                            _startTime = DateTime.Now;
                            
                        }else if( data.value == 1 && _startTime != null){
                            _startTime = null;
                            TimeSpan? durationNullable = DateTime.Now - _startTime;
                            TimeOnly timeStop = TimeOnly.FromTimeSpan(durationNullable.Value);
                            int dataStop = BulatkanKeMenitTerdekat(timeStop);
                            await contex.AddTimeStop(CancellationToken.None, dataStop);


                        }


                        
                    }
                        
                }




                // Notify listener lain
                MessageReceived?.Invoke(payload);
                Console.WriteLine($"[MQTT] Topic: {topic}, Payload: {payload}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MQTT Error] {ex.Message}");
            }
        }
        int BulatkanKeMenitTerdekat(TimeOnly time)
        {
            int menit = time.Minute;
            int detik = time.Second;

            if (detik >= 30) // jika detik >= 30, naik ke menit berikut
                menit++;

            int totalMenit = time.Hour * 60 + menit; // hitung total menit
            return totalMenit;
        }


    }

    // Config MQTT Broker
    public class MqttSettings
    {
        public string Id{ get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Pass { get; set; }
        public string Topik { get; set; }
    }

    // Model JSON MQTT
    public class MqttReceivedMessage
    {
        public string id { get; set; }
        public int value { get; set; }
    }
}
