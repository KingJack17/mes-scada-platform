using FactoryMES.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactoryMES.Core.Interfaces
{
    public interface IProcessService
    {
        Task<IEnumerable<ProcessDto>> GetAllAsync();
        Task<ProcessDto> GetByIdAsync(int id);
        Task<ProcessDto> CreateAsync(CreateOrUpdateProcessDto dto);
        Task UpdateAsync(int id, CreateOrUpdateProcessDto dto);
        Task DeleteAsync(int id);
    }
}