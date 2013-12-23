using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIRLab.Thornado
{
    public partial class TextMold
    {
        public int Line;
        public Type CustomType; //не null, если отличается от заявленного
        public Type ActualType;
        public bool IsNull;
        public string Value;
        public object CorrespondingObject;
        public string Link = null;


        public readonly FieldAddress Address;
        public readonly TextMoldRoot Root;
        private Dictionary<string, TextMold> Nodes = new Dictionary<string, TextMold>();
        public IEnumerable<string> Keys { get { return Nodes.Keys; } }
        public IEnumerable<KeyValuePair<string, TextMold>> Molds { get { return Nodes; } }


        public TextMold GetMold(string key)
        {
            try
            {
                return Nodes[key];
            }
            catch
            {
                throw new Exception("Requested mold is not contained in the collection");
            }
        }

        internal TextMold(FieldAddress address, TextMoldRoot root)
        {
            this.Address = address;
            this.Root = root;
        }

        public TextMold CreateMold(string key)
        {
            var mold = new TextMold(this.Address.Child(key), Root);
            Nodes[key] = mold;
            return mold;
        }
    }
}