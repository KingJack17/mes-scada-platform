using FactoryMES.Core;
using FactoryMES.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FactoryMES.DataAccess.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }
        public IQueryable<User> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }
        public async Task<User> GetUserByIdWithRolesAsync(int id)
        {
            return await _context.Users
                                 .Include(u => u.UserRoles)
                                 .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}