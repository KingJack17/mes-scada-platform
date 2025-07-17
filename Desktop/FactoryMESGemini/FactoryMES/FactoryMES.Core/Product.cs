using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryMES.Core
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string ProductType { get; set; }
        public int MinStockLevel { get; set; }
        public int StockingUnitOfMeasureId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;

        [ForeignKey("StockingUnitOfMeasureId")]
        public virtual UnitOfMeasure StockingUnitOfMeasure { get; set; }
        public virtual ICollection<BomItem> BomParentItems { get; set; }
        public virtual ICollection<BomItem> BomComponentItems { get; set; }
        public virtual ICollection<Route> RouteSteps { get; set; }
    }
}
