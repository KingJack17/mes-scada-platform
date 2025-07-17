using FactoryMES.Core;
using FactoryMES.Core.Interfaces;

namespace FactoryMES.DataAccess.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context) { }
       
        public async Task<Role> GetRoleByIdAsync(int id)
        {
            return await _context.Roles.FindAsync(id);
        }
    }
}