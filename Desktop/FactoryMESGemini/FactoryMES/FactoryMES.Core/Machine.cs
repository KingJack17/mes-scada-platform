using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace FactoryMES.Core
{
    public class Machine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string AssetTag { get; set; }
        public string Status { get; set; }
        public DateTime InstallationDate { get; set; }
        public int MachineTypeId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public double IdealCycleTimeSeconds { get; set; }
        [ForeignKey("MachineTypeId")]
        public virtual MachineType MachineType { get; set; }
        public virtual ICollection<WorkOrder> WorkOrders { get; set; }
        public virtual ICollection<ProductionData> ProductionData { get; set; }
        public virtual ICollection<MachineStatusLog> StatusHistory { get; set; }
        public virtual ICollection<MachineAlarmLog> AlarmHistory { get; set; }
        public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; }
        public virtual ICollection<MaintenanceActivity> MaintenanceActivities { get; set; }
    }
}