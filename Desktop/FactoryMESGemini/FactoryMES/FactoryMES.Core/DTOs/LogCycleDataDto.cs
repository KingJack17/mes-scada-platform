using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FactoryMES.Core.DTOs
{
    public class LogCycleDataDto
    {
        [Required]
        public int MachineId { get; set; }

        public int? WorkOrderId { get; set; }

        public int GoodQuantity { get; set; } = 0;

        public int ScrapQuantity { get; set; } = 0;

        // Proses parametreleri için esnek bir yapi
        public Dictionary<string, object> Parameters { get; set; }
        public double ActualCycleTimeSeconds { get; set; }
    }
}