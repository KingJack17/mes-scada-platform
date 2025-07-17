using FactoryMES.Core;
using FactoryMES.Core.Interfaces;

namespace FactoryMES.DataAccess.Repositories
{
    public class ProductionDataRepository : GenericRepository<ProductionData>, IProductionDataRepository
    {
        public ProductionDataRepository(ApplicationDbContext context) : base(context) { }
    }
}