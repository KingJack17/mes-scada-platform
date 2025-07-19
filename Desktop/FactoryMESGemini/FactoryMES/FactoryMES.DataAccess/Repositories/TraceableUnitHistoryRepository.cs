// TraceableUnitHistoryRepository.cs
using FactoryMES.Core;
using FactoryMES.Core.Interfaces;
namespace FactoryMES.DataAccess.Repositories
{
    public class TraceableUnitHistoryRepository : GenericRepository<TraceableUnitHistory>, ITraceableUnitHistoryRepository
    {
        public TraceableUnitHistoryRepository(ApplicationDbContext context) : base(context) { }
    }
}