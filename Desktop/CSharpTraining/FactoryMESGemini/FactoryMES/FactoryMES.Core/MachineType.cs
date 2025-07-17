using System.Collections.Generic;

namespace FactoryMES.Core
{
    public class MachineType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ParameterDefinition> ParameterDefinitions { get; set; }
    }
}
