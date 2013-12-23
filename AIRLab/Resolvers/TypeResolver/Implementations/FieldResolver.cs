using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIRLab.Thornado
{
    public class FieldResolver<T> : TypeResolver
    {
        public override IEnumerable<string> GetSubaddresses(object obj)
        {
            yield break;
        }

        public override object GetElement(object obj, string sub)
        {
            throw new Exception("There is no children in Fields");
        }

        public override void SetElement(object obj, string sub, object value)
        {
            throw new Exception("There is no children in Fields");
        }

        public override IEnumerable<string> GetDefinedSubaddresses()
        {
            yield break;
        }

        public override Type GetDefinedType(string sub)
        {
            throw new Exception("There is no subaddresses in Fields");
        }

        public override object Create(IEnumerable<KeyValuePair<string, object>> arguments, LogicErrorList list)
        {
            return Activator.CreateInstance(Type);
        }
        
    }
}
