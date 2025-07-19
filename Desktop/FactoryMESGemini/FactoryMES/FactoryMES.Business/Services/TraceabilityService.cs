using FactoryMES.Business.Interfaces;
using FactoryMES.Core.DTOs;
using FactoryMES.Core.Interfaces;
using Microsoft.EntityFrameworkCore; // Include ve ThenInclude için
using System.Linq;
using System.Threading.Tasks;

namespace FactoryMES.Business.Services
{
    public class TraceabilityService : ITraceabilityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TraceabilityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TraceabilityDto> GetTraceabilityBySerialNumberAsync(string serialNumber)
        {
            // 1. Seri numarasına göre izlenecek birimi bul
            var traceableUnit = await _unitOfWork.TraceableUnits.GetQueryable()
                .Include(tu => tu.Product) // İlişkili ürün bilgisini de al
                .FirstOrDefaultAsync(tu => tu.SerialNumber == serialNumber);

            if (traceableUnit == null)
            {
                return null; // Seri numarası bulunamazsa boş dön
            }

            // 2. Bu birime ait tüm geçmiş kayıtlarını, ilişkili verilerle birlikte çek
            var historyLogs = await _unitOfWork.TraceableUnitHistories.GetQueryable()
                .Where(h => h.TraceableUnitId == traceableUnit.Id)
                .Include(h => h.RouteStep).ThenInclude(rs => rs.Process) // Rota Adımı -> Proses
                .Include(h => h.Machine) // Makine
                .Include(h => h.Operator) // Operatör
                .OrderBy(h => h.ProcessStartTime) // Tarihe göre sırala
                .ToListAsync();

            // 3. Çekilen verileri DTO'ya dönüştür
            var traceabilityDto = new TraceabilityDto
            {
                TraceableUnitId = traceableUnit.Id,
                SerialNumber = traceableUnit.SerialNumber,
                BatchNumber = traceableUnit.BatchNumber,
                ProductName = traceableUnit.Product?.Name,
                CurrentStatus = traceableUnit.Status,
                History = historyLogs.Select(log => new TraceabilityHistoryStepDto
                {
                    ProcessName = log.RouteStep?.Process?.Name,
                    MachineName = log.Machine?.Name,
                    OperatorUsername = log.Operator?.Username,
                    ProcessStartTime = log.ProcessStartTime,
                    ProcessEndTime = log.ProcessEndTime,
                    Result = log.Result
                }).ToList()
            };

            return traceabilityDto;
        }
    }
}