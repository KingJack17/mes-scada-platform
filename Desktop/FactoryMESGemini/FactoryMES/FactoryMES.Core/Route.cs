using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryMES.Core
{
    public class Route
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ProcessId { get; set; }
        public int StepNumber { get; set; }
        public decimal IdealCycleTimeSeconds { get; set; }
        // YENİ EKLENEN İLİŞKİ
        [ForeignKey("Machine")]
        public int? MachineId { get; set; }
        public virtual Machine Machine { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [ForeignKey("ProcessId")]
        public virtual Process Process { get; set; }
    }
}