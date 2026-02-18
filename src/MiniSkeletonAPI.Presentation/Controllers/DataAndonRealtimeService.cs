using Microsoft.AspNetCore.SignalR;
using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Presentation.Hubs;

namespace MiniSkeletonAPI.Presentation.Controllers
{
    public class DataAndonRealtimeService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<RealTimeHub> _hubContext;

        public DataAndonRealtimeService(IServiceScopeFactory scopeFactory, IHubContext<RealTimeHub> hubContext)
        {
            _scopeFactory = scopeFactory;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var dataAndonService = scope.ServiceProvider.GetRequiredService<IIdentityDataAndonService>();

                        // 1️⃣ AUTO-CLEAR MINGGUAN
                        var now = DateTime.UtcNow.AddHours(7);
                        if (now.DayOfWeek == DayOfWeek.Monday && now.Hour == 00 && now.Minute == 00)
                        {
                           
                            Console.WriteLine($"🧹 Membersihkan data Andon lama pada {now}");
                            await dataAndonService.UpdateAllDataAndonAsync();
                        }

                        var data = await dataAndonService.GetDataAndonsAsync();


                        if (data != null)
                        {
                            await _hubContext.Clients.All.SendAsync("ReceiveDataAndon", data, cancellationToken: stoppingToken);
                            Console.WriteLine("Data berhasil dikirim ke klien.");
                        }
                        else
                        {
                            Console.WriteLine("Data kosong atau tidak tersedia.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ [SignalR Error] {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }
    }
}
