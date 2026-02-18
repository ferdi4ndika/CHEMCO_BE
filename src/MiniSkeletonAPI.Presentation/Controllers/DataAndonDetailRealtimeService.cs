using Microsoft.AspNetCore.SignalR;
using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Application.Identity.DataAndons.Dtos;
using MiniSkeletonAPI.Presentation.Hubs;
using System.Collections.Generic;
using System.Threading;

namespace MiniSkeletonAPI.Presentation.Controllers
{
    public class DataAndonDetailRealtimeService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<RealTimeHubDetail> _hubContext;

        public DataAndonDetailRealtimeService(IServiceScopeFactory scopeFactory, IHubContext<RealTimeHubDetail> hubContext)
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

                       
                        var data = await dataAndonService.GetDataAndonAsync(stoppingToken);


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
