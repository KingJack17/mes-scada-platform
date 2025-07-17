using FactoryMES.Core;
using FactoryMES.Core.Interfaces;

namespace FactoryMES.DataAccess.Repositories
{
    public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(ApplicationDbContext context) : base(context) { }
    }
}