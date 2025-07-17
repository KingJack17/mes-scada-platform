using FactoryMES.Core;
using FactoryMES.Core.Interfaces;

namespace FactoryMES.DataAccess.Repositories
{
    public class MachineStatusLogRepository : GenericRepository<MachineStatusLog>, IMachineStatusLogRepository
    {
        public MachineStatusLogRepository(ApplicationDbContext context) : base(context) { }
    }
}