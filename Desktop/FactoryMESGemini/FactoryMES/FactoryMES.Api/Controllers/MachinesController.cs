using FactoryMES.Business.Interfaces;
using FactoryMES.Core.DTOs;
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

        public MachinesController(IMachineService machineService)
        {
            _machineService = machineService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMachines()
        {
            var machines = await _machineService.GetAllMachinesAsync();
            return Ok(machines);
        }

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