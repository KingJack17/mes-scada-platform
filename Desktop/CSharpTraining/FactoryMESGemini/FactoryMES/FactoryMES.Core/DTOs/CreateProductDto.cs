using System.ComponentModel.DataAnnotations;

namespace FactoryMES.Core.DTOs
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Ürün kodu zorunludur.")]
        [StringLength(50)]
        public string ProductCode { get; set; }

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