using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryMES.Core
{
    public class MaintenanceRequest
    {
        public int Id { get; set; }
        public int MachineId { get; set; }
        public int ReportedByUserId { get; set; }
        public DateTime RequestDate { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }

        [ForeignKey("MachineId")]
        public virtual Machine Machine { get; set; }
        [ForeignKey("ReportedByUserId")]
        public virtual User ReportedByUser { get; set; }
    }
}