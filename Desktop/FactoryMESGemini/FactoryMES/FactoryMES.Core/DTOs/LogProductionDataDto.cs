using System.ComponentModel.DataAnnotations;

public class LogProductionDataDto
{
    [Required] public int MachineId { get; set; }
    public int? WorkOrderId { get; set; }
    public int GoodQuantity { get; set; } = 0;
    public int ScrapQuantity { get; set; } = 0;
    public double ActualCycleTimeSeconds { get; set; }
}