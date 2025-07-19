using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FactoryMES.Core.DTOs
{
    // Bir ürün rotasındaki tek bir adımı temsil eder (listeleme için)
    public class RouteStepDto
    {
        public int RouteId { get; set; } // Bu, Route tablosundaki satırın ID'sidir
        public int StepNumber { get; set; }
        public int ProcessId { get; set; }
        public string ProcessName { get; set; }
        public int MachineId { get; set; } 
        public string MachineName { get; set; } 
    }

    // Bir ürünün tam rotasını temsil eder (listeleme için)
    public class ProductRouteDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public List<RouteStepDto> Steps { get; set; }
    }

    // Bir ürüne yeni bir rota adımı eklemek için kullanılacak DTO
    public class CreateRouteStepDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int ProcessId { get; set; }
        [Required]
        public int MachineId { get; set; }
        [Required]
        public int StepNumber { get; set; }
    }
    public class UpdateRouteStepDto
    {
        [Required]
        public int ProcessId { get; set; }
        [Required]
        public int MachineId { get; set; }

        [Required]
        public int StepNumber { get; set; }
    }
}