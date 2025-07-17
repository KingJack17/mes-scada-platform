using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryMES.Core
{
    public class MaintenanceActivity
    {
        public int Id { get; set; }
        public int MachineId { get; set; }
        public int? MaintenanceRequestId { get; set; }
        public string ActivityType { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PerformedByUserId { get; set; }
        [ForeignKey("MachineId")]
        public virtual Machine Machine { get; set; }
        [ForeignKey("PerformedByUserId")]
        public virtual User PerformedByUser { get; set; }
        public virtual ICollection<UsedPartLog> UsedParts { get; set; }
    }
}
