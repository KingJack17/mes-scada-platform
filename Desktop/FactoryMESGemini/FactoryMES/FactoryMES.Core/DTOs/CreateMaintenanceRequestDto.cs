using System.ComponentModel.DataAnnotations;

namespace FactoryMES.Core.DTOs
{
    public class CreateMaintenanceRequestDto
    {
        [Required]
        public int MachineId { get; set; }
        [Required]
        public string Description { get; set; }
        public string Priority { get; set; }
    }
}