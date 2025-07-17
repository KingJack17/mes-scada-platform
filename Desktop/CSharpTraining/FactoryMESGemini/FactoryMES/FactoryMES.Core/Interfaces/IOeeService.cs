using FactoryMES.Core.DTOs;
using System;
using System.Threading.Tasks;

namespace FactoryMES.Business.Interfaces
{
    public interface IOeeService
    {
        Task<OeeDto> CalculateOeeForMachineAsync(int machineId, DateTime from, DateTime to);
    }
}