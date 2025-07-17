using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace FactoryMES.Api.Hubs
{
    public class DashboardHub : Hub
    {
        // Bu metot, bir istemci (frontend) bu hub'a bağlandığında çalışır.
        // Test için, birisi bağlandığında backend konsoluna bir mesaj yazdıracağız.
        public override Task OnConnectedAsync()
        {
            System.Console.WriteLine($"--> SignalR İstemcisi Bağlandı: {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            System.Console.WriteLine($"--> SignalR istemcisinin bağlantısı koptu: {Context.ConnectionId}");
            return base.OnDisconnectedAsync(exception);
        }
    }
}