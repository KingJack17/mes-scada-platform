using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryMES.Core
{
    public class ProductionData
    {
        public long Id { get; set; }
        public int WorkOrderId { get; set; }
        public int MachineId { get; set; }
        public int UserId { get; set; }
        public int GoodQuantity { get; set; }
        public int ScrapQuantity { get; set; }
        public DateTime Timestamp { get; set; }
        public double ActualCycleTimeSeconds { get; set; }
        [ForeignKey("WorkOrderId")]
        public virtual WorkOrder WorkOrder { get; set; }
        [ForeignKey("MachineId")]
        public virtual Machine Machine { get; set; }
        [ForeignKey("UserId")]
        public virtual User Operator { get; set; }
    }
}