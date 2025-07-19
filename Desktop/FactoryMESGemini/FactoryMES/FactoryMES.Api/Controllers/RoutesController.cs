using FactoryMES.Core.DTOs;
using FactoryMES.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FactoryMES.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Sadece Admin'ler rota tanımlayabilir
    public class RoutesController : ControllerBase
    {
        private readonly IRouteService _routeService;

        public RoutesController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetRouteForProduct(int productId)
        {
            var route = await _routeService.GetRouteForProductAsync(productId);
            if (route == null) return NotFound("Bu ürüne ait bir rota bulunamadı.");
            return Ok(route);
        }

        [HttpPost]
        public async Task<IActionResult> AddStepToRoute(CreateRouteStepDto dto)
        {
            var newStep = await _routeService.AddStepToRouteAsync(dto);
            return Ok(newStep);
        }

        [HttpDelete("{routeId}")]
        public async Task<IActionResult> DeleteStep(int routeId)
        {
            await _routeService.DeleteStepFromRouteAsync(routeId);
            return NoContent();
        }

        [HttpPut("{routeId}")]
        public async Task<IActionResult> UpdateStep(int routeId, [FromBody] UpdateRouteStepDto dto)
        {
            try
            {
                await _routeService.UpdateStepAsync(routeId, dto);
                return NoContent(); // Başarılı güncelleme sonrası standart yanıt
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}