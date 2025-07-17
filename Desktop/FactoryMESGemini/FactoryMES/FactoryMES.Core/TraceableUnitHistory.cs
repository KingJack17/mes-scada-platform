
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryMES.Core
{
    public class TraceableUnitHistory
    {
        public long Id { get; set; }
        public long TraceableUnitId { get; set; }
        public int RouteId { get; set; }
        public int MachineId { get; set; }
        public int OperatorId { get; set; }
        public DateTime ProcessStartTime { get; set; }
        public DateTime ProcessEndTime { get; set; }
        public string Result { get; set; }

        [ForeignKey("TraceableUnitId")]
        public virtual TraceableUnit TraceableUnit { get; set; }
        [ForeignKey("RouteId")]
        public virtual Route RouteStep { get; set; }
    }
}