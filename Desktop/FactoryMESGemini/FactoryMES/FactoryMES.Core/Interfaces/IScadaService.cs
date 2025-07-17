using FactoryMES.Core.DTOs;
using System.Threading.Tasks;

namespace FactoryMES.Core.Interfaces
{
    public interface IScadaService
    {
        Task LogProductionDataAsync(LogProductionDataDto dto);
        Task LogMachineStatusAsync(LogMachineStatusDto dto);
    }
}