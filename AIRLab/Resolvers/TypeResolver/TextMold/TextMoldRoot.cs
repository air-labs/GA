using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIRLab.Thornado
{

    public class TextMoldRoot
    {
        public readonly TextMold RootMold;
        public Dictionary<object, FieldAddress> Repository = new Dictionary<object, FieldAddress>();
        public TextMoldRoot()
        {
            RootMold = new TextMold(FieldAddress.Root, this);

        }
       
    }
}
