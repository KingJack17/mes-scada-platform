using FactoryMES.Core;
using FactoryMES.Core.DTOs;
using FactoryMES.Core.Interfaces;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FactoryMES.Business.Services
{
    public class ScadaService : IScadaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDashboardNotifier _notifier;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ScadaService(IUnitOfWork unitOfWork, IDashboardNotifier notifier, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _notifier = notifier;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogProductionDataAsync(LogProductionDataDto dto)
        {
            
            var now = DateTime.UtcNow;

            if (dto.GoodQuantity > 0 || dto.ScrapQuantity > 0)
            {
                var productionLog = new ProductionData
                {
                    MachineId = dto.MachineId,
                    GoodQuantity = dto.GoodQuantity,
                    ScrapQuantity = dto.ScrapQuantity,
                    Timestamp = now,
                    
                };

                // DTO'dan gelen WorkOrderId'nin bir değeri var mı diye kontrol et
                if (dto.WorkOrderId.HasValue)
                {
                    // Eğer değeri varsa, entity'ye ata
                    productionLog.WorkOrderId = dto.WorkOrderId.Value;
                }

                await _unitOfWork.ProductionData.AddAsync(productionLog);
            }


            await _unitOfWork.CompleteAsync();

            // 2. Frontend'e anlık veri yayını yapma işini Notifier'a devret
            var cycleDataForNotifier = new LogCycleDataDto
            {
                MachineId = dto.MachineId,
                WorkOrderId = dto.WorkOrderId,
                GoodQuantity = dto.GoodQuantity,
                ScrapQuantity = dto.ScrapQuantity,
                Parameters = new Dictionary<string, object>() // Şimdilik boş bir parametre listesi
            };

            // Notifier'a doğru tipte veri gönderiyoruz.
            await _notifier.SendCycleData(cycleDataForNotifier);

        }

        public async Task LogMachineStatusAsync(LogMachineStatusDto dto)
        {
            // === YENİ EKLENEN VE HATAYI GİDEREN ADIM ===
            // 1. Ana Makine Kaydını Veritabanından Bul
            var machineToUpdate = await _unitOfWork.Machines.GetByIdAsync(dto.MachineId);
            if (machineToUpdate == null)
            {
                // Makine bulunamazsa bir işlem yapma veya hata fırlat
                // Şimdilik sessizce çıkıyoruz.
                return;
            }
            // 2. Makinenin mevcut durumunu DTO'dan gelen yeni durumla güncelle
            machineToUpdate.Status = dto.NewStatus;
            // Not: UnitOfWork, bu değişikliği zaten takip ediyor ve SaveChanges'de veritabanına yansıtacak.
            // === GÜNCELLEME ADIMI BİTİŞİ ===


            // Önceki durum logunu bul ve bitiş zamanını ayarla
            var lastStatus = (await _unitOfWork.MachineStatusLogs
                .FindAsync(s => s.MachineId == dto.MachineId && s.EndTime == null))
                .FirstOrDefault();

            if (lastStatus != null)
            {
                lastStatus.EndTime = DateTime.UtcNow;
            }

            // Yeni durum logu kaydını oluştur
            var newStatusLog = new MachineStatusLog
            {
                MachineId = dto.MachineId,
                Status = dto.NewStatus,
                StartTime = DateTime.UtcNow,
                EndTime = null, // Bu durum devam ediyor
                Notes = dto.Notes
            };

            await _unitOfWork.MachineStatusLogs.AddAsync(newStatusLog);

            // CompleteAsync, hem makine güncellemesini hem de yeni log kaydını tek bir işlemde veritabanına yazar.
            await _unitOfWork.CompleteAsync();

            // Frontend'e anlık olarak yeni makine durumunu yayınla
            await _notifier.SendMachineStatusUpdate(dto.MachineId, dto.NewStatus);
        }
    }
}