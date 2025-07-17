using System;

namespace FactoryMES.Core.DTOs
{
    public class MaintenanceRequestDto
    {
        public int Id { get; set; }
        public string MachineName { get; set; }
        public string ReportedByUserName { get; set; }
        public DateTime RequestDate { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
    }
}
