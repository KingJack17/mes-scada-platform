using FactoryMES.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactoryMES.Business.Interfaces
{
    public interface IWorkOrderService
    {
        Task<IEnumerable<WorkOrderDto>> GetAllAsync();
        Task<WorkOrderDto> GetByIdAsync(int id);
        Task<WorkOrderDto> CreateAsync(CreateWorkOrderDto dto);
        Task<bool> UpdateStatusAsync(int id, UpdateWorkOrderStatusDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(int id, UpdateWorkOrderDto dto);

    }
}