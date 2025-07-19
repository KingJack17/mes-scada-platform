using FactoryMES.Core;
using FactoryMES.Core.Interfaces;

namespace FactoryMES.DataAccess.Repositories
{
    public class ProcessRepository : GenericRepository<Process>, IProcessRepository
    {
        public ProcessRepository(ApplicationDbContext context) : base(context) { }
    }
}