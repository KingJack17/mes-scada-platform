using FactoryMES.Core.Interfaces;
using FactoryMES.Core.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FactoryMES.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScadaController : ControllerBase
    {
        private readonly IScadaService _scadaService;
        public ScadaController(IScadaService scadaService)
        {
            _scadaService = scadaService;
        }

        [HttpPost("status")]
        public async Task<IActionResult> PostMachineStatus([FromBody] LogMachineStatusDto dto)
        {
            await _scadaService.LogMachineStatusAsync(dto);
            return Ok();
        }
    }
}