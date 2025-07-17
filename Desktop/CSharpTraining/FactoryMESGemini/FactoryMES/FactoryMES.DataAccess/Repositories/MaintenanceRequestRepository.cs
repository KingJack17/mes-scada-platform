using FactoryMES.Core;
using FactoryMES.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactoryMES.DataAccess.Repositories
{
    public class MaintenanceRequestRepository : GenericRepository<MaintenanceRequest>, IMaintenanceRequestRepository
    {
        public MaintenanceRequestRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<IEnumerable<MaintenanceRequest>> GetAllAsync()
        {
            // İlişkili Makine ve Kullanıcı bilgilerini de getiriyoruz.
            return await _context.MaintenanceRequests
                                 .Include(r => r.Machine)
                                 .Include(r => r.ReportedByUser)
                                 .ToListAsync();
        }
    }
}