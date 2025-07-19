using FactoryMES.Core;

namespace FactoryMES.Core.Interfaces
{
    public interface IRouteRepository : IGenericRepository<Route>
    {
        IQueryable<Route> GetQueryable();
    }
}