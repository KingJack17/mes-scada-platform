public class UpdateWorkOrderDto
{
    public int Id { get; set; }
    public string OrderNumber { get; set; }
    public int ProductId { get; set; }
    public int MachineId { get; set; }
    public int PlannedQuantity { get; set; }
    public DateTime PlannedStartDate { get; set; }
    public DateTime PlannedEndDate { get; set; }
    public string Status { get; set; }
}
