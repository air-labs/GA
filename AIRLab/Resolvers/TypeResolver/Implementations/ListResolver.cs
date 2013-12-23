using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace AIRLab.Thornado
{
    public class ListResolver<T> : TypeResolver
    {
        public override IEnumerable<string> GetSubaddresses(object obj)
        {
            for (int i = 0; i < ((IList)obj).Count; i++) yield return i.ToString();
        }

        public override object GetElement(object obj, string sub)
        {
            return ((IList<T>)obj)[Formats.Int.Parse(sub)];
        }

        public override void SetElement(object obj, string sub, object value)
        {
            ((IList<T>)obj)[Formats.Int.Parse(sub)] = (T)value;
        }

        public override IEnumerable<string> GetDefinedSubaddresses()
        {
            yield return "*";
        }

        public override Type GetDefinedType(string sub)
        {
            return typeof(T);
        }

        public override object Create(IEnumerable<KeyValuePair<string, object>> mold, LogicErrorList errors)
        {
            var list = (IList<T>)Type.GetConstructor(new Type[0]).Invoke(new object[0]);
            foreach (var e in mold)
            {
                int ind = 0;
                try
                {
                    ind = int.Parse(e.Key);
                }
                catch
                {
                    errors.Add(LogicErrorLevel.Error, "Неверный индекс " + e + " в листе");
                    continue;
                }
                while (list.Count <= ind)
                    list.Add(default(T));
                list[ind] = (T)e.Value;
            }
            return list;
        }

    }
}
