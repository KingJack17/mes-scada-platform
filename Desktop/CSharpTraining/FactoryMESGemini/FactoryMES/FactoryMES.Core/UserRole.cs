using System.ComponentModel.DataAnnotations.Schema;
namespace FactoryMES.Core
{
    public class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
}