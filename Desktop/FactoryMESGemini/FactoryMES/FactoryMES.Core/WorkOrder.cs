using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryMES.Core
{
    public class WorkOrder
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public int PlannedQuantity { get; set; }
        public int ActualQuantity { get; set; }
        public string Status { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; } // Yabancı Anahtar
        public virtual Product Product { get; set; } // Navigasyon Özelliği

        [ForeignKey("Machine")]
        public int MachineId { get; set; } // Yabancı Anahtar
        public virtual Machine Machine { get; set; } // Navigasyon Özelliği
    }
}