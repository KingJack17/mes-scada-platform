using FactoryMES.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactoryMES.Core.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersWithRolesAsync();
        Task<bool> AssignRoleAsync(int userId, int roleId);
        Task<bool> RemoveRoleAsync(int userId, int roleId);
        Task<bool> DeleteUserAsync(int id);
    }
}