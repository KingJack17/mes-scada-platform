using FactoryMES.Business.Interfaces;
using FactoryMES.Core;
using FactoryMES.Core.DTOs;
using FactoryMES.Core.Interfaces;
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
                InstallationDate = System.DateTime.UtcNow,
                IsDeleted = false
            };

            await _unitOfWork.Machines.AddAsync(machine);
            await _unitOfWork.CompleteAsync();

            // Kaydedilen makineyi DTO olarak geri dön
            return new MachineDto
            {
                Id = machine.Id,
                Name = machine.Name,
                // Diğer alanları da buraya ekleyebiliriz...
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
            var machines = await _unitOfWork.Machines.GetAllAsync();

            // Entity listesini DTO listesine çeviriyoruz (Mapping)
            return machines.Select(m => new MachineDto
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Location = m.Location,
                AssetTag = m.AssetTag,
                Status = m.Status,
                MachineTypeId = m.MachineTypeId,
                MachineTypeName = m.MachineType?.Name //?. null kontrolü yapar
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
                MachineTypeName = machine.MachineType?.Name
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

            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}