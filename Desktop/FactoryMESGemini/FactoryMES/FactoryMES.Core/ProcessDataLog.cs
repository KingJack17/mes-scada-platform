using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryMES.Core
{
    public class ProcessDataLog
    {
        public long Id { get; set; }
        public long TraceableUnitHistoryId { get; set; }
        public DateTime Timestamp { get; set; }

        [Column(TypeName = "jsonb")]
        public string ParametersJson { get; set; }

        [ForeignKey("TraceableUnitHistoryId")]
        public virtual TraceableUnitHistory TraceableUnitHistory { get; set; }
    }
}