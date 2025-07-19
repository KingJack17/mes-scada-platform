using FactoryMES.Core.DTOs;
using FactoryMES.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FactoryMES.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Sadece Admin'ler proses tanımlayabilir
    public class ProcessesController : ControllerBase
    {
        private readonly IProcessService _processService;
        public ProcessesController(IProcessService processService)
        {
            _processService = processService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var processes = await _processService.GetAllAsync();
            return Ok(processes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var process = await _processService.GetByIdAsync(id);
            if (process == null) return NotFound();
            return Ok(process);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrUpdateProcessDto dto)
        {
            var createdProcess = await _processService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdProcess.Id }, createdProcess);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateOrUpdateProcessDto dto)
        {
            await _processService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _processService.DeleteAsync(id);
            return NoContent();
        }
    }
}