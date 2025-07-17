using FactoryMES.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactoryMES.Business.Interfaces
{
    public interface IMachineService
    {
        Task<IEnumerable<MachineDto>> GetAllMachinesAsync();
        Task<MachineDto> GetMachineByIdAsync(int id);
        Task<MachineDto> CreateMachineAsync(CreateOrUpdateMachineDto machineDto);
        Task<bool> UpdateMachineAsync(int id, CreateOrUpdateMachineDto machineDto);
        Task<bool> DeleteMachineAsync(int id);
    }
}