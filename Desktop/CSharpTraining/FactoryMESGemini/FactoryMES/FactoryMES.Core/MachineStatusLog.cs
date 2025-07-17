using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryMES.Core
{
    public class MachineStatusLog
    {
        public long Id { get; set; }
        public int MachineId { get; set; }
        public string Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Notes { get; set; }
        [ForeignKey("MachineId")]
        public virtual Machine Machine { get; set; }
    }
}