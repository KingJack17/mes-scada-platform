// TraceableUnitRepository.cs
using FactoryMES.Core;
using FactoryMES.Core.Interfaces;
namespace FactoryMES.DataAccess.Repositories
{
    public class TraceableUnitRepository : GenericRepository<TraceableUnit>, ITraceableUnitRepository
    {
        public TraceableUnitRepository(ApplicationDbContext context) : base(context) { }
    }
}