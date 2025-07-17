namespace FactoryMES.Core.DTOs
{
    public class OeeDto
    {
        // Değerler % olarak (0-100 arası)
        public double Availability { get; set; }
        public double Performance { get; set; }
        public double Quality { get; set; }
        public double OverallOee { get; set; }
        public string Status { get; set; }
    }
}