using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryMES.Core
{
    public class Inventory
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int StockLocationId { get; set; }
        public int Quantity { get; set; }
        public DateTime LastUpdated { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [ForeignKey("StockLocationId")]
        public virtual StockLocation StockLocation { get; set; }
    }
}
