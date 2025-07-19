using FactoryMES.Core;
using FactoryMES.Core.DTOs;
using FactoryMES.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryMES.Business.Services
{
    public class RouteService : IRouteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RouteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<RouteStepDto> AddStepToRouteAsync(CreateRouteStepDto dto)
        {
            var routeStep = new Route
            {
                ProductId = dto.ProductId,
                ProcessId = dto.ProcessId,
                MachineId = dto.MachineId,
                StepNumber = dto.StepNumber
            };

            await _unitOfWork.Routes.AddAsync(routeStep);
            await _unitOfWork.CompleteAsync();

            // DTO dönüşümü için ilişkili verileri çek
            var process = await _unitOfWork.Processes.GetByIdAsync(dto.ProcessId);
            var machine = await _unitOfWork.Machines.GetByIdAsync(dto.MachineId);

            return new RouteStepDto
            {
                RouteId = routeStep.Id,
                StepNumber = routeStep.StepNumber,
                ProcessId = routeStep.ProcessId,
                ProcessName = process?.Name,
                MachineId = routeStep.MachineId.GetValueOrDefault(),
                MachineName = machine?.Name
            };
        }

        public async Task DeleteStepFromRouteAsync(int routeId)
        {
            var routeStep = await _unitOfWork.Routes.GetByIdAsync(routeId);
            if (routeStep == null) throw new Exception("Rota adımı bulunamadı.");

            _unitOfWork.Routes.Remove(routeStep);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<ProductRouteDto> GetRouteForProductAsync(int productId)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product == null) return null;

            var steps = await _unitOfWork.Routes.GetQueryable()
                                       .Where(r => r.ProductId == productId)
                                       .Include(r => r.Process)
                                       .Include(r => r.Machine) // MAKİNE BİLGİSİNİ DAHİL ET
                                       .OrderBy(r => r.StepNumber)
                                       .Select(r => new RouteStepDto
                                       {
                                           RouteId = r.Id,
                                           StepNumber = r.StepNumber,
                                           ProcessId = r.ProcessId,
                                           ProcessName = r.Process != null ? r.Process.Name : null,
                                           MachineId = r.MachineId.GetValueOrDefault(),
                                           MachineName = r.Machine != null ? r.Machine.Name : null // MAKİNE ADINI EKLE
                                       })
                                       .ToListAsync();

            return new ProductRouteDto
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Steps = steps
            };
        }

        public async Task UpdateStepAsync(int routeId, UpdateRouteStepDto dto)
        {
            var routeStep = await _unitOfWork.Routes.GetByIdAsync(routeId);
            if (routeStep == null)
            {
                throw new Exception("Güncellenecek rota adımı bulunamadı.");
            }

            // DTO'dan gelen yeni verilerle entity'yi güncelle
            routeStep.ProcessId = dto.ProcessId;
            routeStep.MachineId = dto.MachineId;
            routeStep.StepNumber = dto.StepNumber;

            _unitOfWork.Routes.Update(routeStep);
            await _unitOfWork.CompleteAsync();
        }
    }
}