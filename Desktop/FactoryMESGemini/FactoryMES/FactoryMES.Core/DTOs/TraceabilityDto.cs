using System;
using System.Collections.Generic;

namespace FactoryMES.Core.DTOs
{
    // Bir TraceableUnit'in geçmişindeki tek bir adımı temsil eder.
    public class TraceabilityHistoryStepDto
    {
        public string ProcessName { get; set; } // Hangi işlemden geçti (Kesim, Montaj vb.)
        public string MachineName { get; set; } // Hangi makinede işlendi
        public string OperatorUsername { get; set; } // Hangi operatör tarafından işlendi
        public DateTime ProcessStartTime { get; set; }
        public DateTime ProcessEndTime { get; set; }
        public string Result { get; set; } // Başarılı, Başarısız
    }

    // Bir ürünün veya partinin tam izlenebilirlik geçmişini temsil eder.
    public class TraceabilityDto
    {
        public long TraceableUnitId { get; set; }
        public string SerialNumber { get; set; }
        public string BatchNumber { get; set; }
        public string ProductName { get; set; }
        public string CurrentStatus { get; set; }
        public List<TraceabilityHistoryStepDto> History { get; set; }

        public TraceabilityDto()
        {
            History = new List<TraceabilityHistoryStepDto>();
        }
    }
}