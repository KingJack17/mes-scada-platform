
using FactoryMES.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }
    IMachineRepository Machines { get; }
    IWorkOrderRepository WorkOrders { get; }
    IMaintenanceRequestRepository MaintenanceRequests { get; }
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }
    IUserRoleRepository UserRoles { get; }
    IProductionDataRepository ProductionData { get; }
    IMachineStatusLogRepository MachineStatusLogs { get; }
    ITraceableUnitRepository TraceableUnits { get; }
    ITraceableUnitHistoryRepository TraceableUnitHistories { get; }
    IProcessRepository Processes { get; }
    IRouteRepository Routes { get; }
    Task<int> CompleteAsync();
}