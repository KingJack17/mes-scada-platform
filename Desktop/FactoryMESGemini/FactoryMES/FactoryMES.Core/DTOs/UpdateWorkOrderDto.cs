using System;
using System.ComponentModel.DataAnnotations;

namespace FactoryMES.Core.DTOs
{
    public class UpdateWorkOrderDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string OrderNumber { get; set; }
        [Required]
        public int ProductId { get; set; }
       
        [Required]
        [Range(1, int.MaxValue)]
        public int PlannedQuantity { get; set; }
        public string PlannedStartDate { get; set; }
        public string PlannedEndDate { get; set; }
        public string Status { get; set; }
    }
}