using FactoryMES.Business.Interfaces;
using FactoryMES.Core.DTOs;
using FactoryMES.Core.Interfaces; // IUnitOfWork için eklendi
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FactoryMES.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MachinesController : ControllerBase
    {
        private readonly IMachineService _machineService;
        private readonly IUnitOfWork _unitOfWork; // Sorgulama için UnitOfWork eklendi

        // Constructor güncellendi
        public MachinesController(IMachineService machineService, IUnitOfWork unitOfWork)
        {
            _machineService = machineService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetMachines()
        {
            var machines = await _machineService.GetAllMachinesAsync();
            return Ok(machines);
        }

        // === YENİ EKLENEN ENDPOINT ===
        [HttpGet("by-process/{processId}")]
        public async Task<IActionResult> GetMachinesByProcess(int processId)
        {
            // Doğrudan UnitOfWork üzerinden, bir prosese ait olan ve silinmemiş makineleri buluyoruz.
            var machines = await _unitOfWork.Machines.FindAsync(m => m.ProcessId == processId && !m.IsDeleted);
            return Ok(machines);
        }
        // === BİTİŞ ===

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMachine(int id)
        {
            var machine = await _machineService.GetMachineByIdAsync(id);
            if (machine == null) return NotFound();
            return Ok(machine);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMachine(CreateOrUpdateMachineDto machineDto)
        {
            var createdMachine = await _machineService.CreateMachineAsync(machineDto);
            return CreatedAtAction(nameof(GetMachine), new { id = createdMachine.Id }, createdMachine);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMachine(int id, CreateOrUpdateMachineDto machineDto)
        {
            var success = await _machineService.UpdateMachineAsync(id, machineDto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMachine(int id)
        {
            var success = await _machineService.DeleteMachineAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}