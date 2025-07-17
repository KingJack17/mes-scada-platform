using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace FactoryMES.Core
{
    public class ParameterDefinition
    {
        public int Id { get; set; }
        public int MachineTypeId { get; set; }
        public string Name { get; set; }
        public int UnitOfMeasureId { get; set; }

        [ForeignKey("MachineTypeId")]
        public virtual MachineType MachineType { get; set; }
        [ForeignKey("UnitOfMeasureId")]
        public virtual UnitOfMeasure UnitOfMeasure { get; set; }
    }
}