using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryMES.Core
{
    public class KanbanSignal
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string SignalType { get; set; }
        public DateTime SignalDate { get; set; }
        public string Status { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}