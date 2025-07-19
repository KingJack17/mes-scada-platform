using FactoryMES.Core.DTOs;
using System.Threading.Tasks;

namespace FactoryMES.Business.Interfaces
{
    public interface ITraceabilityService
    {
        Task<TraceabilityDto> GetTraceabilityBySerialNumberAsync(string serialNumber);
    }
}