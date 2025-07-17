using FactoryMES.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactoryMES.Business.Interfaces
{
    public interface IMaintenanceService
    {
        Task<IEnumerable<MaintenanceRequestDto>> GetAllRequestsAsync();
        Task<MaintenanceRequestDto> CreateRequestAsync(CreateMaintenanceRequestDto dto, int userId);
        Task<bool> UpdateRequestStatusAsync(int id, UpdateMaintenanceRequestStatusDto dto);
        Task<bool> DeleteRequestAsync(int id);
    }
}
