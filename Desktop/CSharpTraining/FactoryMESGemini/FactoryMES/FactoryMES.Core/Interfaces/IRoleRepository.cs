using FactoryMES.Core;

namespace FactoryMES.Core.Interfaces
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<Role> GetRoleByIdAsync(int id);
    }
}