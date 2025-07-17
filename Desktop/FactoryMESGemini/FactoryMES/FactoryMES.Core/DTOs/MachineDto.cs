namespace FactoryMES.Core.DTOs
{
    public class MachineDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string AssetTag { get; set; }
        public string Status { get; set; }
        public string MachineTypeName { get; set; } // ID yerine isim göstereceğiz.
        public int MachineTypeId { get; set; }
    }
}