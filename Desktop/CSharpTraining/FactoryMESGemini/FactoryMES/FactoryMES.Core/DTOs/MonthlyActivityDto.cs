using System.Collections.Generic;

namespace FactoryMES.Core.DTOs
{
    public class MonthlyActivityDto
    {
        public List<string> Months { get; set; } // E.g., ["Ocak", "Şubat", ...]
        public List<int> WorkOrderCounts { get; set; }
        public List<int> MaintenanceRequestCounts { get; set; }
    }
}