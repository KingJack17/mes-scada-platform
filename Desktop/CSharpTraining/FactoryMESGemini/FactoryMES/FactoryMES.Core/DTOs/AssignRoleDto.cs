using System.ComponentModel.DataAnnotations;

namespace FactoryMES.Core.DTOs
{
    public class AssignRoleDto
    {
        [Required(ErrorMessage = "Rol ID'si zorunludur.")]
        public int RoleId { get; set; }
    }
}