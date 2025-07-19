using System.ComponentModel.DataAnnotations;

namespace FactoryMES.Core.DTOs
{
    public class CreateOrUpdateMachineDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string AssetTag { get; set; }
        public string Status { get; set; }
        [Required]
        public int MachineTypeId { get; set; }

        [Required(ErrorMessage = "Proses ID'si zorunludur.")]
        public int ProcessId { get; set; }
    }
}