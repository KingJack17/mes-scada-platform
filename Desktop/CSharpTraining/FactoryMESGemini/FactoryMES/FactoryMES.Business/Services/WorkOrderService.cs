using FactoryMES.Business.Interfaces;
using FactoryMES.Core;
using FactoryMES.Core.DTOs;
using FactoryMES.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryMES.Business.Services
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WorkOrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<WorkOrderDto>> GetAllAsync()
        {
            var workOrders = await _unitOfWork.WorkOrders.GetAllAsync();
            return workOrders.Select(wo => new WorkOrderDto
            {
                Id = wo.Id,
                OrderNumber = wo.OrderNumber,
                ProductName = wo.Product?.Name,
                MachineName = wo.Machine?.Name,
                PlannedQuantity = wo.PlannedQuantity,
                ActualQuantity = wo.ActualQuantity,
                Status = wo.Status,
                PlannedStartDate = wo.PlannedStartDate,
                PlannedEndDate = wo.PlannedEndDate
            });
        }

        public async Task<WorkOrderDto> GetByIdAsync(int id)
        {
            var workOrder = await _unitOfWork.WorkOrders.GetByIdAsync(id);
            if (workOrder == null) return null;
            // Benzer şekilde DTO'ya map'leyip dönebiliriz.
            return new WorkOrderDto { /* ... alanları doldur ... */ };
        }

        public async Task<WorkOrderDto> CreateAsync(CreateWorkOrderDto dto)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);
            if (product == null) throw new System.Exception($"ID'si {dto.ProductId} olan ürün bulunamadı.");

            var machine = await _unitOfWork.Machines.GetByIdAsync(dto.MachineId);
            if (machine == null) throw new System.Exception($"ID'si {dto.MachineId} olan makine bulunamadı.");

            var workOrder = new WorkOrder
            {
                OrderNumber = dto.OrderNumber,
                ProductId = dto.ProductId,
                MachineId = dto.MachineId,
                PlannedQuantity = dto.PlannedQuantity,
                ActualQuantity = 0,
                Status = "Planlandı",
                PlannedStartDate = System.DateTime.Parse(dto.PlannedStartDate).ToUniversalTime(),
                PlannedEndDate = System.DateTime.Parse(dto.PlannedEndDate).ToUniversalTime()
            };

            await _unitOfWork.WorkOrders.AddAsync(workOrder);
            await _unitOfWork.CompleteAsync();

            // Artık GetByIdAsync ilişkili verileri de getirdiği için bu dönüşüm doğru çalışacak.
            var createdWorkOrder = await _unitOfWork.WorkOrders.GetByIdAsync(workOrder.Id);

            return new WorkOrderDto
            {
                Id = createdWorkOrder.Id,
                OrderNumber = createdWorkOrder.OrderNumber,
                ProductName = createdWorkOrder.Product?.Name,
                MachineName = createdWorkOrder.Machine?.Name,
                PlannedQuantity = createdWorkOrder.PlannedQuantity,
                ActualQuantity = createdWorkOrder.ActualQuantity,
                Status = createdWorkOrder.Status,
                PlannedStartDate = createdWorkOrder.PlannedStartDate,
                PlannedEndDate = createdWorkOrder.PlannedEndDate
            };
        }

        public async Task<bool> UpdateStatusAsync(int id, UpdateWorkOrderStatusDto dto)
        {
            var workOrder = await _unitOfWork.WorkOrders.GetByIdAsync(id);
            if (workOrder == null) return false;

            workOrder.Status = dto.Status;
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var workOrder = await _unitOfWork.WorkOrders.GetByIdAsync(id);
            if (workOrder == null) return false;

            _unitOfWork.WorkOrders.Remove(workOrder);
            await _unitOfWork.CompleteAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(int id, UpdateWorkOrderDto dto)
        {
            var workOrder = await _unitOfWork.WorkOrders.GetByIdAsync(id);
            if (workOrder == null) return false;

            workOrder.OrderNumber = dto.OrderNumber;
            workOrder.ProductId = dto.ProductId;
            workOrder.MachineId = dto.MachineId;
            workOrder.PlannedQuantity = dto.PlannedQuantity;
            workOrder.PlannedStartDate = dto.PlannedStartDate;
            workOrder.PlannedEndDate = dto.PlannedEndDate;
            workOrder.Status = dto.Status;

            _unitOfWork.WorkOrders.Update(workOrder); 
            await _unitOfWork.CompleteAsync();

            return true;
        }


    }
}