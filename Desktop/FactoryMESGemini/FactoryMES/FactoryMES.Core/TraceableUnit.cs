using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryMES.Core
{
    public class TraceableUnit
    {
        public long Id { get; set; }
        public int ProductId { get; set; }
        public string SerialNumber { get; set; }
        public string BatchNumber { get; set; }
        public string Status { get; set; }
        public int? CurrentRouteStepId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}