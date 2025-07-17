using FactoryMES.Core.DTOs;
using FactoryMES.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FactoryMES.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Bu kontrolcüye SADECE Admin'ler erişebilir.
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
       
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersWithRolesAsync();
            return Ok(users);
        }

        [HttpPost("{userId}/roles")]
        public async Task<IActionResult> AssignRole(int userId, [FromBody] AssignRoleDto dto)
        {
            var success = await _userService.AssignRoleAsync(userId, dto.RoleId);
            if (!success) return NotFound("Kullanıcı veya rol bulunamadı.");
            return Ok();
        }

        [HttpDelete("{userId}/roles/{roleId}")]
        public async Task<IActionResult> RemoveRole(int userId, int roleId)
        {
            var success = await _userService.RemoveRoleAsync(userId, roleId);
            if (!success) return NotFound("Kullanıcıya ait bu rol bulunamadı.");
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await _userService.DeleteUserAsync(id);
            if (!success)
            {
                return NotFound($"ID'si {id} olan kullanıcı bulunamadı.");
            }
            return NoContent(); // Başarılı silme sonrası standart yanıt
        }
    }
}