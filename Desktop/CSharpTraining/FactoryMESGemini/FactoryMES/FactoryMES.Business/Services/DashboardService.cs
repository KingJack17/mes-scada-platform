using FactoryMES.Business.Interfaces;
using FactoryMES.Core.DTOs;
using FactoryMES.Core.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryMES.Business.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DashboardSummaryDto> GetSummaryAsync()
        {
            // Sorguları artık paralel değil, arka arkaya (sıralı) çalıştırıyoruz.
            // Her bir 'await', o sorgunun bitmesini bekler ve sonra diğerine geçer.
            var allMachines = await _unitOfWork.Machines.GetAllAsync();
            var openWorkOrders = await _unitOfWork.WorkOrders.FindAsync(wo => wo.Status == "Planlandı" || wo.Status == "Üretimde");
            var highPrioRequests = await _unitOfWork.MaintenanceRequests.FindAsync(r => r.Status == "Açık" && r.Priority == "Yüksek");

            // Hesaplamaları yap
            var summary = new DashboardSummaryDto
            {
                TotalMachines = allMachines.Count(),
                RunningMachines = allMachines.Count(m => m.Status == "Running"),
                OpenWorkOrders = openWorkOrders.Count(),
                HighPriorityMaintenanceRequests = highPrioRequests.Count()
            };

            return summary;
        }
        public async Task<MonthlyActivityDto> GetMonthlyActivityAsync()
        {
            var result = new MonthlyActivityDto
            {
                Months = new List<string>(),
                WorkOrderCounts = new List<int>(),
                MaintenanceRequestCounts = new List<int>()
            };

            var today = DateTime.UtcNow;
            for (int i = 11; i >= 0; i--)
            {
                var date = today.AddMonths(-i);
                var monthName = new System.Globalization.CultureInfo("tr-TR").DateTimeFormat.GetMonthName(date.Month);
                result.Months.Add(monthName);

                var workOrdersInMonth = await _unitOfWork.WorkOrders.FindAsync(wo => wo.PlannedStartDate.Month == date.Month && wo.PlannedStartDate.Year == date.Year);
                result.WorkOrderCounts.Add(workOrdersInMonth.Count());

                var requestsInMonth = await _unitOfWork.MaintenanceRequests.FindAsync(r => r.RequestDate.Month == date.Month && r.RequestDate.Year == date.Year);
                result.MaintenanceRequestCounts.Add(requestsInMonth.Count());
            }

            return result;
        }
        public async Task<DailyProductionDto> GetDailyProductionAsync(int lastDays)
        {
            var result = new DailyProductionDto
            {
                Days = new List<string>(),
                GoodPartCounts = new List<int>(),
                ScrapPartCounts = new List<int>()
            };

            var startDate = DateTime.UtcNow.Date.AddDays(-lastDays + 1);

            var productionData = await _unitOfWork.ProductionData
                .FindAsync(pd => pd.Timestamp >= startDate);

            var dailyGroups = productionData
                .GroupBy(pd => pd.Timestamp.Date)
                .ToDictionary(g => g.Key, g => new
                {
                    Good = g.Sum(pd => pd.GoodQuantity),
                    Scrap = g.Sum(pd => pd.ScrapQuantity)
                });

            for (int i = 0; i < lastDays; i++)
            {
                var day = startDate.AddDays(i);
                result.Days.Add(day.ToString("dd.MM"));

                if (dailyGroups.ContainsKey(day))
                {
                    result.GoodPartCounts.Add(dailyGroups[day].Good);
                    result.ScrapPartCounts.Add(dailyGroups[day].Scrap);
                }
                else
                {
                    result.GoodPartCounts.Add(0);
                    result.ScrapPartCounts.Add(0);
                }
            }

            return result;
        }
    }
}