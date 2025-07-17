using FactoryMES.Business.Interfaces;
using FactoryMES.Core;
using FactoryMES.Core.DTOs;
using FactoryMES.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using System.Security.Claims;

namespace FactoryMES.Business.Services
{
    public class OeeService : IOeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDashboardNotifier _notifier;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OeeService(IUnitOfWork unitOfWork, IDashboardNotifier notifier, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _notifier = notifier;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OeeDto> CalculateOeeForMachineAsync(int machineId, DateTime from, DateTime to)
        {
            // 1. Makine bilgisi al
            var machine = await _unitOfWork.Machines.GetByIdAsync(machineId);
            if (machine == null)
                throw new Exception("Makine bulunamadı.");

            // 2. Durum loglarını ve üretim verilerini al
            var statusLogs = await _unitOfWork.MachineStatusLogs.FindAsync(log =>
                log.MachineId == machineId &&
                log.StartTime < to &&
                (log.EndTime == null || log.EndTime > from));

            var productionData = await _unitOfWork.ProductionData.FindAsync(pd =>
                pd.MachineId == machineId &&
                pd.Timestamp >= from &&
                pd.Timestamp <= to);

            // 3. Availability hesapla
            double totalTimeInSeconds = (to - from).TotalSeconds;
            double plannedDownTime = 0; // Gelecekte eklenebilir
            double runningTime = statusLogs
            .Where(log => log.Status == "Running")
            .Sum(log =>
            {
            var start = log.StartTime < from ? from : log.StartTime;
            var end = (log.EndTime ?? to) > to ? to : (log.EndTime ?? to);
            return (end - start).TotalSeconds;
            });

            double availability = (totalTimeInSeconds > 0)
                ? (runningTime / (totalTimeInSeconds - plannedDownTime)) * 100
                : 0;

            // 4. Quality hesapla
            int totalGoodParts = productionData.Sum(pd => pd.GoodQuantity);
            int totalScrapParts = productionData.Sum(pd => pd.ScrapQuantity);
            int totalParts = totalGoodParts + totalScrapParts;

            double quality = (totalParts > 0)
                ? ((double)totalGoodParts / totalParts) * 100
                : 100; // Üretim yoksa kalite %100

            // 5. Performance hesapla
            double idealCycleTime = machine.IdealCycleTimeSeconds;
            double performance = 100; // Varsayılan

            if (totalParts > 0 && runningTime > 0 && idealCycleTime > 0)
            {
                double theoreticalPartCount = runningTime / idealCycleTime;
                performance = (totalParts / theoreticalPartCount) * 100;
            }

            // 6. OEE hesapla
            double overallOee = (availability / 100) * (performance / 100) * (quality / 100) * 100;
            // Machine Status
            var latestStatus = await _unitOfWork.MachineStatusLogs
            .FindAsync(log => log.MachineId == machineId);
            var currentStatus = latestStatus
                .OrderByDescending(log => log.StartTime)
                .FirstOrDefault()?.Status ?? "Unknown";
            return new OeeDto
            {
                Availability = Math.Round(availability, 2),
                Performance = Math.Round(performance, 2),
                Quality = Math.Round(quality, 2),
                OverallOee = Math.Round(overallOee, 2),
                Status = currentStatus
            };
        }
        public async Task LogProductionAndBroadcastOeeAsync(LogProductionDataDto dto)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString))
            {
                // Login olmamış birisi bu isteği gönderemez ama yine de bir güvenlik kontrolü.
                throw new Exception("Kullanıcı kimliği bulunamadı.");
            }
            var userId = int.Parse(userIdString);

            var productionLog = new ProductionData
            {
                MachineId = dto.MachineId,
                GoodQuantity = dto.GoodQuantity,
                ScrapQuantity = dto.ScrapQuantity,
                Timestamp = DateTime.UtcNow,
                ActualCycleTimeSeconds = dto.ActualCycleTimeSeconds,
                UserId = userId // 
            };

            // DTO'dan gelen WorkOrderId'nin bir değeri var mı diye kontrol et
            if (dto.WorkOrderId.HasValue)
            {
                productionLog.WorkOrderId = dto.WorkOrderId.Value;
            }

            await _unitOfWork.ProductionData.AddAsync(productionLog);
            await _unitOfWork.CompleteAsync();

            // OEE'yi son 1 saat için yeniden hesapla
            var to = DateTime.UtcNow;
            var from = to.AddHours(-1);
            var oeeData = await CalculateOeeForMachineAsync(dto.MachineId, from, to);

            Console.WriteLine("➡️ SignalR: OEE yayını yapılıyor", oeeData.OverallOee);
            // Hesaplanan yeni OEE skorunu SignalR üzerinden yayınla
            await _notifier.SendOeeUpdate(dto.MachineId, oeeData);
        }
    }
}
