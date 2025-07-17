using FactoryMES.Business.Interfaces;
using FactoryMES.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FactoryMES.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkOrdersController : ControllerBase
    {
        private readonly IWorkOrderService _workOrderService;

        public WorkOrdersController(IWorkOrderService workOrderService)
        {
            _workOrderService = workOrderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWorkOrders()
        {
            var workOrders = await _workOrderService.GetAllAsync();
            return Ok(workOrders);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkOrder([FromBody] CreateWorkOrderDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var createdWorkOrder = await _workOrderService.CreateAsync(dto);
                return Ok(createdWorkOrder); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkOrder(int id)
        {
            var success = await _workOrderService.DeleteAsync(id);
            if (!success)
            {
                return NotFound($"ID'si {id} olan iş emri bulunamadı.");
            }
            return NoContent(); 
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateWorkOrderStatus(int id, [FromBody] UpdateWorkOrderStatusDto dto)
        {
            var success = await _workOrderService.UpdateStatusAsync(id, dto);
            if (!success)
            {
                return NotFound($"ID'si {id} olan iş emri bulunamadı.");
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkOrder(int id, [FromBody] UpdateWorkOrderDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("ID uyuşmuyor.");
            }

            var success = await _workOrderService.UpdateAsync(id, dto);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }



    }
}