using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIRLab.Thornado
{
    public class ThornadoAttribute : Attribute, IADAttribute
    {
        string description;
        string caption;

        public ThornadoAttribute(string capt = "")
        {
            this.caption = this.description = capt == null ? "" : capt;
        }

        public ThornadoAttribute(string capt, string desc)
        {
            this.caption = capt==null?"":capt;
            this.description = desc==null?"":desc;
        }

        public IEnumerable<ADDefinition> DefineAD(Type appliedType)
        {
            yield return AD.Caption.Define(caption);
            yield return AD.Description.Define(description);
        }
    }

 

}
