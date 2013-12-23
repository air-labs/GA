using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIRLab.Thornado
{
    public class ADDefinition
    {
        public readonly ADSpecification ADSpec;
        public readonly object ADValue;
        public ADDefinition(ADSpecification spec, object ADValue)
        {
            this.ADSpec = spec;
            this.ADValue = ADValue;
        }
    }

    public interface IADAttribute
    {
         IEnumerable<ADDefinition> DefineAD(Type appliedType);
    }
}
