using FactoryMES.Business.Interfaces;
using FactoryMES.Core;
using FactoryMES.Core.DTOs;
using FactoryMES.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryMES.Business.Services
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly IUnitOfWork _unitOfWork;

        // Artık IHttpContextAccessor'a ihtiyacımız yok, constructor'dan kaldırıldı.
        public MaintenanceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<MaintenanceRequestDto>> GetAllRequestsAsync()
        {
            var requests = await _unitOfWork.MaintenanceRequests.GetAllAsync();
            return requests.Select(r => new MaintenanceRequestDto
            {
                Id = r.Id,
                MachineName = r.Machine?.Name,
                ReportedByUserName = r.ReportedByUser?.Username,
                RequestDate = r.RequestDate,
                Description = r.Description,
                Priority = r.Priority,
                Status = r.Status
            });
        }
        // Metodun imzası değişti, artık 'userId' parametresi alıyor.
        public async Task<MaintenanceRequestDto> CreateRequestAsync(CreateMaintenanceRequestDto dto, int userId)
        {
            var request = new MaintenanceRequest
            {
                MachineId = dto.MachineId,
                ReportedByUserId = userId, // Parametreden gelen güvenli ID'yi kullanıyoruz
                Description = dto.Description,
                Priority = dto.Priority ?? "Normal",
                RequestDate = DateTime.UtcNow,
                Status = "Açık"
            };

            await _unitOfWork.MaintenanceRequests.AddAsync(request);
            await _unitOfWork.CompleteAsync();

            var createdRequest = await _unitOfWork.MaintenanceRequests.GetByIdAsync(request.Id);
            // ... DTO dönüşümü ...
            return new MaintenanceRequestDto { Id = createdRequest.Id, Description = createdRequest.Description };
        }

        public async Task<bool> UpdateRequestStatusAsync(int id, UpdateMaintenanceRequestStatusDto dto)
        {
            var request = await _unitOfWork.MaintenanceRequests.GetByIdAsync(id);
            if (request == null) return false;

            request.Status = dto.Status;
            await _unitOfWork.CompleteAsync();
            return true;
        }
        public async Task<bool> DeleteRequestAsync(int id)
        {
            var request = await _unitOfWork.MaintenanceRequests.GetByIdAsync(id);
            if (request == null) return false;

            _unitOfWork.MaintenanceRequests.Remove(request);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
 