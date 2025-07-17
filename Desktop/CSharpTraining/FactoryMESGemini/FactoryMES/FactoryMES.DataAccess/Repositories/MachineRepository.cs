using FactoryMES.Core;
using FactoryMES.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryMES.DataAccess.Repositories
{
    public class MachineRepository : GenericRepository<Machine>, IMachineRepository
    {
        public MachineRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<IEnumerable<Machine>> GetAllAsync()
        {
            return await _context.Machines
                                 .Where(m => !m.IsDeleted)
                                 .Include(m => m.MachineType)
                                 .AsNoTracking()
                                 .ToListAsync();
        }

        public override async Task<Machine> GetByIdAsync(int id)
        {
            return await _context.Machines
                                 .Include(m => m.MachineType)
                                 .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
        }

        public override void Remove(Machine entity)
        {
            entity.IsDeleted = true;
            _context.Machines.Update(entity);
        }
    }
}