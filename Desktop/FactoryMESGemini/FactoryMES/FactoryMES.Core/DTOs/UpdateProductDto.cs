using System.ComponentModel.DataAnnotations;

namespace FactoryMES.Core.DTOs
{
    public class UpdateProductDto
    {
        [Required(ErrorMessage = "Ürün adı zorunludur.")]
        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        public string ProductType { get; set; }

        [Range(0, int.MaxValue)]
        public int MinStockLevel { get; set; }

        [Required]
        public int StockingUnitOfMeasureId { get; set; }
    }
}