using System.ComponentModel.DataAnnotations;

namespace FactoryMES.Core.DTOs
{
    public class CreateOrUpdateProcessDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}