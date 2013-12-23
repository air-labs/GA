using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace AIRLab.Thornado
{
    class ConstructorData
    {
        public ConstructorInfo ReflectedCons { get; private set; }
        public List<string> ParamNames { get; private set; }
        public Delegate FakedCons { get; private set; }
        public int ParamCount { get { return ParamNames.Count; } }
        public ConstructorData(ConstructorInfo cs)
        {
            ReflectedCons = cs;
            ParamNames=cs.GetParameters().Select(z=>z.Name).ToList();
        }

        public ConstructorData(Delegate del)
        {
            ReflectedCons = null;
            FakedCons = del;
            ParamNames = del.Method.GetParameters().Select(z => z.Name).ToList();
        }

        public ConstructorInvocation CreateInvocation() { return new ConstructorInvocation(this); }
    }

    class ConstructorInvocation
    {
        int ParamLeft;
        public bool Ready { get { return ParamLeft==0; }}
        object[] arguments;
        public ConstructorData ContructorData { get; private set; }
        public ConstructorInvocation(ConstructorData data)
        {
            ContructorData = data;
            ParamLeft = data.ParamCount;
            arguments = new object[ParamLeft];
        }
        public object Invoke()
        {
            if (!Ready) throw new Exception("");
            if (ContructorData.ReflectedCons != null)
                return ContructorData.ReflectedCons.Invoke(arguments);
            else
                return ContructorData.FakedCons.DynamicInvoke(arguments);
        }

        public void MatchParam(string name, object value)
        {
            var index = ContructorData.ParamNames.IndexOf(name);
            if (index != -1)
            {
                ParamLeft--;
                arguments[index] = value;
            }
        }

    }
}
