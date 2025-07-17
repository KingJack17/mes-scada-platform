using FactoryMES.Core.DTOs;
using System.Threading.Tasks;

namespace FactoryMES.Business.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetSummaryAsync();
        Task<MonthlyActivityDto> GetMonthlyActivityAsync();
        Task<DailyProductionDto> GetDailyProductionAsync(int lastDays);
    }
}