using FactoryMES.Core;

namespace FactoryMES.Core.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        IQueryable<User> GetQueryable();
        Task<User> GetUserByIdWithRolesAsync(int id);
    }
}