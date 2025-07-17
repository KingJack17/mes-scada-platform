using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryMES.Core
{
    public class MachineAlarmLog
    {
        public long Id { get; set; }
        public int MachineId { get; set; }
        public string AlarmCode { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsCleared { get; set; }
        public DateTime? ClearedTimestamp { get; set; }

        [ForeignKey("MachineId")]
        public virtual Machine Machine { get; set; }
    }
}