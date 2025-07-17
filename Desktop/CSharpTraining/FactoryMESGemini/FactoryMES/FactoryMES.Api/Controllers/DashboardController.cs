using FactoryMES.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FactoryMES.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Bu endpoint'lere sadece giriş yapmış kullanıcılar erişebilir.
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetDashboardSummary()
        {
            var summaryData = await _dashboardService.GetSummaryAsync();
            return Ok(summaryData);
        }
        [HttpGet("monthly-activity")]
        public async Task<IActionResult> GetMonthlyActivity()
        {
            var activityData = await _dashboardService.GetMonthlyActivityAsync();
            return Ok(activityData);
        }

        [HttpGet("daily-production/{lastDays}")]
        public async Task<IActionResult> GetDailyProduction(int lastDays = 30)
        {
            var productionData = await _dashboardService.GetDailyProductionAsync(lastDays);
            return Ok(productionData);
        }
    }
}