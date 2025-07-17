using System.ComponentModel.DataAnnotations;

namespace FactoryMES.Core.DTOs
{
    public class CreateWorkOrderDto
    {
        [Required]
        public string OrderNumber { get; set; }
        [Required]
        public int ProductId { get; set; } // Hangi ürünün üretileceği
        [Required]
        public int MachineId { get; set; } // Hangi makinede üretileceği
        [Required]
        [Range(1, int.MaxValue)]
        public int PlannedQuantity { get; set; }
        public string PlannedStartDate { get; set; }
        public string PlannedEndDate { get; set; }
    }
}