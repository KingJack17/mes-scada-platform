using System.Collections.Generic;

namespace FactoryMES.Core.DTOs
{
    public class DailyProductionDto
    {
        public List<string> Days { get; set; } // E.g., ["01.07", "02.07", ...]
        public List<int> GoodPartCounts { get; set; }
        public List<int> ScrapPartCounts { get; set; }
    }
}