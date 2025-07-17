using System.ComponentModel.DataAnnotations;
namespace FactoryMES.Core.DTOs
{
    public class UpdateWorkOrderStatusDto
    {
        [Required]
        public string Status { get; set; }
    }
}