using FactoryMES.Core;
using FactoryMES.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FactoryMES.DataAccess.Repositories
{
    public class RouteRepository : GenericRepository<Route>, IRouteRepository
    {
        public RouteRepository(ApplicationDbContext context) : base(context) { }
        public override IQueryable<Route> GetQueryable()
        {
            return _dbSet.Include(r => r.Process);
        }
    }
}