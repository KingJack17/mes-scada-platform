using FactoryMES.Core;
using FactoryMES.Core.DTOs;
using FactoryMES.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryMES.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersWithRolesAsync()
        {
            var users = await _unitOfWork.Users.GetQueryable()
                                    .Include(u => u.UserRoles)
                                    .ThenInclude(ur => ur.Role)
                                    .ToListAsync();

            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                IsActive = u.IsActive,
                SicilNo = u.SicilNo,
                Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
            });
        }
        public async Task<bool> AssignRoleAsync(int userId, int roleId)
        {
            // 1. Kullanıcıyı rolleriyle birlikte veritabanından bul
            var user = await _unitOfWork.Users.GetUserByIdWithRolesAsync(userId);
            if (user == null) throw new Exception("Kullanıcı bulunamadı.");

            // 2. Rolü veritabanından bul
            var role = await _unitOfWork.Roles.GetRoleByIdAsync(roleId);
            if (role == null) throw new Exception("Rol bulunamadı.");

            // 3. Kullanıcının bu role zaten sahip olup olmadığını kontrol et
            if (user.UserRoles.Any(ur => ur.RoleId == roleId))
            {
                // Zaten sahipse bir şey yapma
                return true;
            }

            // 4. Yeni UserRole nesnesini oluştur
            var newUserRole = new UserRole
            {
                UserId = userId,
                RoleId = roleId,
            };

            // Bu nesneyi direkt DbContext üzerinden eklemek daha güvenilirdir.
            await _unitOfWork.UserRoles.AddAsync(newUserRole);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> RemoveRoleAsync(int userId, int roleId)
        {
            var userRole = (await _unitOfWork.UserRoles.FindAsync(ur => ur.UserId == userId && ur.RoleId == roleId)).FirstOrDefault();
            if (userRole == null)
            {
                // Kullanıcının böyle bir rolü yoksa hata dön
                return false;
            }

            _unitOfWork.UserRoles.Remove(userRole);
            await _unitOfWork.CompleteAsync();
            return true;
        }
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null) return false; // Kullanıcı bulunamadı

            _unitOfWork.Users.Remove(user); // Bu GenericRepository'deki fiziksel silme metodunu kullanır
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}