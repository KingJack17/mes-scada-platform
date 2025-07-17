using System;

namespace FactoryMES.Core.DTOs
{
    public class WorkOrderDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string ProductName { get; set; } // ID yerine İSİM
        public string MachineName { get; set; } // ID yerine İSİM
        public int PlannedQuantity { get; set; }
        public int ActualQuantity { get; set; }
        public string Status { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
    }
}