using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryMES.Core
{
    public class BomItem
    {
        public int Id { get; set; }
        public int ParentProductId { get; set; }
        public int ComponentProductId { get; set; }
        public decimal Quantity { get; set; }
        public int UnitOfMeasureId { get; set; }
        [ForeignKey("ParentProductId")]
        public virtual Product ParentProduct { get; set; }
        [ForeignKey("ComponentProductId")]
        public virtual Product ComponentProduct { get; set; }
        [ForeignKey("UnitOfMeasureId")]
        public virtual UnitOfMeasure UnitOfMeasure { get; set; }
    }
}