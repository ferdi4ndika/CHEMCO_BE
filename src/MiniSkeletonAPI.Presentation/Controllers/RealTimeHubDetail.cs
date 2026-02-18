using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Presentation.Hubs
{
    public class RealTimeHubDetail : Hub
    {
        public async Task SendMessage(string message)
        {
            Console.WriteLine("HubDetail received: " + message);
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
