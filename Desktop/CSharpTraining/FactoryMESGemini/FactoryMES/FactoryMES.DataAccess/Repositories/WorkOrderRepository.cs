using FactoryMES.Core;
using FactoryMES.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactoryMES.DataAccess.Repositories
{
    public class WorkOrderRepository : GenericRepository<WorkOrder>, IWorkOrderRepository
    {
        public WorkOrderRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<IEnumerable<WorkOrder>> GetAllAsync()
        {
            return await _context.WorkOrders
                                 .Include(wo => wo.Product)
                                 .Include(wo => wo.Machine)
                                 .ToListAsync();
        }

        // YENİ EKLENEN OVERRIDE METODU
        public override async Task<WorkOrder> GetByIdAsync(int id)
        {
            return await _context.WorkOrders
                                 .Include(wo => wo.Product)
                                 .Include(wo => wo.Machine)
                                 .FirstOrDefaultAsync(wo => wo.Id == id);
        }
    }
}