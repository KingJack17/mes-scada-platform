using FactoryMES.Business.Interfaces;
using FactoryMES.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FactoryMES.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OeeController : ControllerBase
    {
        private readonly IOeeService _oeeService;

        public OeeController(IOeeService oeeService)
        {
            _oeeService = oeeService;
        }

        [HttpGet("{machineId}")]
        // Değişiklik 1: Parametreleri string olarak alıyoruz.
        public async Task<IActionResult> GetOeeForMachine(int machineId, [FromQuery] string from, [FromQuery] string to)
        {
            try
            {
                // Değişiklik 2: Gelen string tarihleri DateTime'a çevirip hemen UTC yapıyoruz.
                var fromDate = DateTime.Parse(from).ToUniversalTime();
                var toDate = DateTime.Parse(to).ToUniversalTime();

                // Değişiklik 3: Servise artık güvenli olan bu tarihleri gönderiyoruz.
                var oeeData = await _oeeService.CalculateOeeForMachineAsync(machineId, fromDate, toDate);
                return Ok(oeeData);
            }
            catch (Exception ex)
            {
                // Makine bulunamadı veya tarih formatı hatalı gibi hataları yakalamak için
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("log-production")]
        public async Task<IActionResult> LogProduction([FromBody] LogProductionDataDto dto)
        {
            await _oeeService.LogProductionAndBroadcastOeeAsync(dto);
            return Ok(new { message = "Production data logged and OEE broadcasted." });
        }
    }
}