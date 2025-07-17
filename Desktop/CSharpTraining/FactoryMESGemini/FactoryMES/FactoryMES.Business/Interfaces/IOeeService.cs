using FactoryMES.Core.DTOs;

public interface IOeeService
{
    Task<OeeDto> CalculateOeeForMachineAsync(int machineId, DateTime from, DateTime to);
    Task LogProductionAndBroadcastOeeAsync(LogProductionDataDto dto); 
}