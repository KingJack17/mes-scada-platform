using System.ComponentModel.DataAnnotations;

namespace FactoryMES.Core.DTOs
{
    public class LogMachineStatusDto
    {
        [Required]
        public int MachineId { get; set; }

        [Required]
        public string NewStatus { get; set; } // Örn: "Running", "Stopped", "Error"

        public string Notes { get; set; }
    }
}