
using FactoryMES.Core.Interfaces;
using System.Threading.Tasks;

namespace FactoryMES.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IProductRepository Products { get; private set; }
        public IMachineRepository Machines { get; private set; }
        public IWorkOrderRepository WorkOrders { get; private set; }
        public IMaintenanceRequestRepository MaintenanceRequests { get; private set; }
        public IUserRepository Users { get; private set; }
        public IRoleRepository Roles { get; private set; }
        public IUserRoleRepository UserRoles { get; }
        public IProductionDataRepository ProductionData { get; private set; }
        public IMachineStatusLogRepository MachineStatusLogs { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Products = new ProductRepository(_context); 
            Machines = new MachineRepository(_context);
            WorkOrders = new WorkOrderRepository(_context);
            MaintenanceRequests = new MaintenanceRequestRepository(_context);
            Users = new UserRepository(_context);
            Roles = new RoleRepository(_context);
            UserRoles = new UserRoleRepository(_context);
            ProductionData = new ProductionDataRepository(_context);
            MachineStatusLogs = new MachineStatusLogRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}