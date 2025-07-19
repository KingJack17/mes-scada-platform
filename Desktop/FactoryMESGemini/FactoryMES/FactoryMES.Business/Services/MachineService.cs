using FactoryMES.Business.Interfaces;
using FactoryMES.Core;
using FactoryMES.Core.DTOs;
using FactoryMES.Core.Interfaces;
using Microsoft.EntityFrameworkCore; // Include için eklendi
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryMES.Business.Services
{
    public class MachineService : IMachineService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MachineService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<MachineDto> CreateMachineAsync(CreateOrUpdateMachineDto machineDto)
        {
            var machine = new Machine
            {
                Name = machineDto.Name,
                Description = machineDto.Description,
                Location = machineDto.Location,
                AssetTag = machineDto.AssetTag,
                Status = machineDto.Status,
                MachineTypeId = machineDto.MachineTypeId,
                ProcessId = machineDto.ProcessId, // YENİ EKLENDİ
                InstallationDate = System.DateTime.UtcNow,
                IsDeleted = false
            };

            await _unitOfWork.Machines.AddAsync(machine);
            await _unitOfWork.CompleteAsync();

            // Kaydedilen makineyi tam DTO olarak geri dön
            var createdMachine = await _unitOfWork.Machines.GetByIdAsync(machine.Id);
            return new MachineDto
            {
                Id = createdMachine.Id,
                Name = createdMachine.Name,
                Description = createdMachine.Description,
                Location = createdMachine.Location,
                AssetTag = createdMachine.AssetTag,
                Status = createdMachine.Status,
                MachineTypeId = createdMachine.MachineTypeId,
                MachineTypeName = createdMachine.MachineType?.Name,
                ProcessId = createdMachine.ProcessId,
                ProcessName = createdMachine.Process?.Name
            };
        }

        public async Task<bool> DeleteMachineAsync(int id)
        {
            var machine = await _unitOfWork.Machines.GetByIdAsync(id);
            if (machine == null) return false;

            _unitOfWork.Machines.Remove(machine); // Soft delete
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<IEnumerable<MachineDto>> GetAllMachinesAsync()
        {
            // Sorguya Process'i de dahil ediyoruz.
            var machines = await _unitOfWork.Machines.GetQueryable()
                                       .Include(m => m.MachineType)
                                       .Include(m => m.Process) 
                                       .Where(m => !m.IsDeleted)
                                       .ToListAsync();

            // DTO'ya Process bilgisini de ekliyoruz.
            return machines.Select(m => new MachineDto
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Location = m.Location,
                AssetTag = m.AssetTag,
                Status = m.Status,
                MachineTypeId = m.MachineTypeId,
                MachineTypeName = m.MachineType?.Name,
                ProcessId = m.ProcessId, 
                ProcessName = m.Process?.Name 
            });
        }

        public async Task<MachineDto> GetMachineByIdAsync(int id)
        {
            var machine = await _unitOfWork.Machines.GetByIdAsync(id);
            if (machine == null) return null;

            return new MachineDto
            {
                Id = machine.Id,
                Name = machine.Name,
                Description = machine.Description,
                Location = machine.Location,
                AssetTag = machine.AssetTag,
                Status = machine.Status,
                MachineTypeId = machine.MachineTypeId,
                MachineTypeName = machine.MachineType?.Name,
                ProcessId = machine.ProcessId, 
                ProcessName = machine.Process?.Name 
            };
        }

        public async Task<bool> UpdateMachineAsync(int id, CreateOrUpdateMachineDto machineDto)
        {
            var machineFromDb = await _unitOfWork.Machines.GetByIdAsync(id);
            if (machineFromDb == null) return false;

            // Alanları güncelle
            machineFromDb.Name = machineDto.Name;
            machineFromDb.Description = machineDto.Description;
            machineFromDb.Location = machineDto.Location;
            machineFromDb.AssetTag = machineDto.AssetTag;
            machineFromDb.Status = machineDto.Status;
            machineFromDb.MachineTypeId = machineDto.MachineTypeId;
            machineFromDb.ProcessId = machineDto.ProcessId; 

            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}