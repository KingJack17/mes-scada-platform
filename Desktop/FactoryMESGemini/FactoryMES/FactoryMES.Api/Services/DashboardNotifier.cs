using FactoryMES.Api.Hubs;
using FactoryMES.Core.DTOs;
using FactoryMES.Core.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace FactoryMES.Api.Services
{
    public class DashboardNotifier : IDashboardNotifier
    {
        private readonly IHubContext<DashboardHub> _hubContext;

        public DashboardNotifier(IHubContext<DashboardHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendCycleData(LogCycleDataDto cycleData)
        {
            // "ReceiveCycleData" adıyla tüm bağlı istemcilere cycleData'yı gönderir.
            await _hubContext.Clients.All.SendAsync("ReceiveCycleData", cycleData);
        }

        public async Task SendMachineStatusUpdate(int machineId, string newStatus)
        {
            // "ReceiveMachineStatusUpdate" adıyla tüm bağlı istemcilere makine ID'sini ve yeni durumu gönderir.
            await _hubContext.Clients.All.SendAsync("ReceiveMachineStatusUpdate", machineId, newStatus);
        }
        public async Task SendOeeUpdate(int machineId, OeeDto oeeData)
        {
            Console.WriteLine("🔔 SignalR Yayını: " + JsonConvert.SerializeObject(oeeData));
            await _hubContext.Clients.All.SendAsync("ReceiveOeeUpdate", machineId, oeeData);
        }
    }
}