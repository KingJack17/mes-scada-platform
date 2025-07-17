using System.Collections.Generic;

namespace FactoryMES.Core
{
    public class Process
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Route> RouteSteps { get; set; }
}
}