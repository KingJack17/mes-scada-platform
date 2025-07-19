using FactoryMES.Core;
using FactoryMES.Core.DTOs;
using FactoryMES.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryMES.Business.Services
{
    public class ProcessService : IProcessService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProcessService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProcessDto> CreateAsync(CreateOrUpdateProcessDto dto)
        {
            var process = new Process
            {
                Name = dto.Name,
                Description = dto.Description
            };
            await _unitOfWork.Processes.AddAsync(process);
            await _unitOfWork.CompleteAsync();
            return new ProcessDto { Id = process.Id, Name = process.Name, Description = process.Description };
        }

        public async Task DeleteAsync(int id)
        {
            var process = await _unitOfWork.Processes.GetByIdAsync(id);
            if (process == null) throw new Exception("Proses bulunamadı.");

            _unitOfWork.Processes.Remove(process);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<ProcessDto>> GetAllAsync()
        {
            var processes = await _unitOfWork.Processes.GetAllAsync();
            return processes.Select(p => new ProcessDto { Id = p.Id, Name = p.Name, Description = p.Description });
        }

        public async Task<ProcessDto> GetByIdAsync(int id)
        {
            var process = await _unitOfWork.Processes.GetByIdAsync(id);
            if (process == null) return null;
            return new ProcessDto { Id = process.Id, Name = process.Name, Description = process.Description };
        }

        public async Task UpdateAsync(int id, CreateOrUpdateProcessDto dto)
        {
            var process = await _unitOfWork.Processes.GetByIdAsync(id);
            if (process == null) throw new Exception("Proses bulunamadı.");

            process.Name = dto.Name;
            process.Description = dto.Description;
            _unitOfWork.Processes.Update(process);
            await _unitOfWork.CompleteAsync();
        }
    }
}