using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace AIRLab.Thornado
{
    public class DictionaryResolver<K,T> : TypeResolver
    {
        static TypeFormat<K> KeyFormat;

        static DictionaryResolver()
        {
            KeyFormat = (TypeFormat<K>)TypeFormat.GetDefaultFormat(typeof(K));
        }

        public override IEnumerable<string> GetSubaddresses(object obj)
        {
            return ((IDictionary<K, T>)obj).Keys.Select(z => KeyFormat.Write(z));
        }

        public override object GetElement(object obj, string sub)
        {
            return ((IDictionary<K, T>)obj)[KeyFormat.Parse(sub)];
        }

        public override void SetElement(object obj, string sub, object value)
        {
            ((IDictionary<K, T>)obj)[KeyFormat.Parse(sub)] = (T)value;
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
            var dict = (IDictionary)Type.GetConstructor(new Type[0]).Invoke(new object[0]);
            foreach (var e in mold)
            {
                K key = default(K);
                try
                {
                    key = KeyFormat.Parse(e.Key);
                }
                catch (Exception t)
                {
                    errors.Add(LogicErrorLevel.Error, "Неверный ключ словаря " + e);
                    continue;
                }
                dict[key] = e.Value;
            }
            return dict;
        }
    }
}
