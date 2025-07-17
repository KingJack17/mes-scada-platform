using FactoryMES.Core.DTOs;
using System.Threading.Tasks;

namespace FactoryMES.Core.Interfaces
{
    public interface IDashboardNotifier
    {
        Task SendMachineStatusUpdate(int machineId, string newStatus);
        Task SendCycleData(LogCycleDataDto cycleData);
        Task SendOeeUpdate(int machineId, OeeDto oeeData);
    }
}