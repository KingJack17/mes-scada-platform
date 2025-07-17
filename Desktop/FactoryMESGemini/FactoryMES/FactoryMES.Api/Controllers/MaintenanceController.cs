using FactoryMES.Business.Interfaces;
using FactoryMES.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FactoryMES.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {
        private readonly IMaintenanceService _maintenanceService;

        public MaintenanceController(IMaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService;
        }

        [HttpGet("requests")]
        public async Task<IActionResult> GetRequests()
        {
            var requests = await _maintenanceService.GetAllRequestsAsync();
            return Ok(requests);
        }

        [HttpPost("requests")]
        [Authorize] // Bu endpoint'in login gerektirdiğinden emin oluyoruz
        public async Task<IActionResult> CreateRequest([FromBody] CreateMaintenanceRequestDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Controller, kendi 'User' özelliğinden ID'yi okur.
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("Token içinde kullanıcı kimliği bulunamadı.");
            }

            var userId = int.Parse(userIdString);

            try
            {
                // Servise ID'yi bir parametre olarak geçer.
                var createdRequest = await _maintenanceService.CreateRequestAsync(dto, userId);
                return Ok(createdRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("requests/{id}/status")]
        public async Task<IActionResult> UpdateRequestStatus(int id, [FromBody] UpdateMaintenanceRequestStatusDto dto)
        {
            var success = await _maintenanceService.UpdateRequestStatusAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("requests/{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var success = await _maintenanceService.DeleteRequestAsync(id);
            if (!success)
            {
                return NotFound($"ID'si {id} olan bakım talebi bulunamadı.");
            }
            return NoContent(); // Başarılı silme sonrası standart yanıt
        }
    }
}
