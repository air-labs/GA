using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIRLab.Thornado
{
    public delegate void ContextDependedParser(TextMold mold, Type type, LogicErrorList list);

    partial class TypeResolver
    {
        public static void ParseMold(Type expectedType, TextMold mold, LogicErrorList list, ContextDependedParser cdp)
        {
            if (mold==null) return;
            if (mold.CustomType != null) expectedType = mold.CustomType;
            if (cdp!=null) cdp(mold, expectedType, list);
            if (mold.IsNull) { mold.CorrespondingObject = null; return; }
            var resolver=TypeResolver.GetResolver(expectedType);
            if (resolver.Category == Categories.Field)
            {
                mold.CorrespondingObject = resolver.Format.ParseObject(mold.Value);
                return;
            }
            foreach(var e in mold.Molds)
            {
                var type=resolver.GetDefinedType(e.Key);
                ParseMold(type,e.Value,list,cdp);
            }
            mold.CorrespondingObject=resolver.Create(
                mold.Molds.Select(z=>new KeyValuePair<string,object>(z.Key,z.Value.CorrespondingObject)),
                list);
        }

        

        public void WriteToMold(TextMold mold, object obj)
        {
            mold.CorrespondingObject = obj;
            
            if (obj == null)
                mold.IsNull = true;
            else
            {
                mold.IsNull = false;
                mold.ActualType = obj.GetType();
                if (Format != null)
                    mold.Value = Format.WriteObject(obj);
                else
                    foreach (var e in GetSubaddresses(obj))
                    {
                        var ch = GetElement(obj, e);
                        TextMold chmold = mold.CreateMold(e);
                        if (ch == null)
                        {
                            chmold.IsNull = true;
                        }
                        else
                        {
                            var chres = TypeResolver.GetResolver(ch.GetType());
                            chres.WriteToMold(chmold, ch);
                            if (chres.Type != GetDefinedType(e))
                                chmold.CustomType = chres.Type;
                        }
                    }
            }
        }

       
    }
}