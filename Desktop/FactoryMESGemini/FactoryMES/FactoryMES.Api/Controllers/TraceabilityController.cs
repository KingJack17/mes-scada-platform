using FactoryMES.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FactoryMES.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Bu endpoint'lere sadece giriş yapmış kullanıcılar erişebilir.
    public class TraceabilityController : ControllerBase
    {
        private readonly ITraceabilityService _traceabilityService;

        public TraceabilityController(ITraceabilityService traceabilityService)
        {
            _traceabilityService = traceabilityService;
        }

        // Örnek: /api/traceability/by-serial?serialNumber=SN12345
        [HttpGet("by-serial")]
        public async Task<IActionResult> GetBySerialNumber([FromQuery] string serialNumber)
        {
            if (string.IsNullOrEmpty(serialNumber))
            {
                return BadRequest("Seri numarası gereklidir.");
            }

            var traceabilityData = await _traceabilityService.GetTraceabilityBySerialNumberAsync(serialNumber);

            if (traceabilityData == null)
            {
                return NotFound($"'{serialNumber}' seri numarasına sahip bir ürün bulunamadı.");
            }

            return Ok(traceabilityData);
        }
    }
}