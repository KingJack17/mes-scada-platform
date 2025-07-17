using System.ComponentModel.DataAnnotations;
namespace FactoryMES.Core.DTOs
{
    public class UpdateMaintenanceRequestStatusDto
    {
        [Required]
        public string Status { get; set; }
    }
}