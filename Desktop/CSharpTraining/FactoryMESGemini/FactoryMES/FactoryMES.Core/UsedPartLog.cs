using System.ComponentModel.DataAnnotations.Schema;
namespace FactoryMES.Core
{
    public class UsedPartLog
    {
        public int Id { get; set; }
        public int MaintenanceActivityId { get; set; }
        public int ProductId { get; set; }
        public int QuantityUsed { get; set; }
        [ForeignKey("MaintenanceActivityId")]
        public virtual MaintenanceActivity MaintenanceActivity { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}