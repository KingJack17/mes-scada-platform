namespace FactoryMES.Core.DTOs
{
    public class DashboardSummaryDto
    {
        public int TotalMachines { get; set; }
        public int RunningMachines { get; set; }
        public int OpenWorkOrders { get; set; }
        public int HighPriorityMaintenanceRequests { get; set; }
    }
}