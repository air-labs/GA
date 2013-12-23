using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using AIRLab.Thornado;

namespace AIRLab.Thornado
{
    interface IChopRule
    {
        bool Make(ref Expression currentExpression, List<string> accumulatedAddress, ref bool stop);
    }

    class ChopRule<T> : IChopRule
        where T: Expression
    {
        string name;
        Func<T,bool> appliable;
        Func<T,Expression> performChop;
        Func<T,string> getWord;
        bool isFinal;

        public override string ToString()
        {
            return name;
        }
        public ChopRule(string name,Func<T,bool> appliable, Func<T,Expression> performChop, Func<T,string> getWord, bool isFinal)
        {
            this.name = name;
            this.appliable=appliable;
            this.performChop=performChop;
            this.getWord=getWord;
            this.isFinal=isFinal;
        }
        public bool Make(ref Expression currentExpression, List<string> accumulatedAddress, ref bool stop)
        {
            var tp = typeof(T);
            if (!(currentExpression is T)) return false;
            if (!appliable((T)currentExpression)) return false;
            var word=getWord((T)currentExpression);
            if (word!=null)
                accumulatedAddress.Add(word);
            currentExpression = performChop((T)currentExpression);
            stop = isFinal;
            return true;
        }
    }


    public class ExpressionToAddressConverter
    {
        

        static IEnumerable<IChopRule> GetRules()
        {

            yield return new ChopRule<MethodCallExpression>(
                "*",
              z => z.NodeType == ExpressionType.Call && z.Method.Name == "CommonAD",
              z => z.Arguments[0],
              z => "*",
              false);

            yield return new ChopRule<UnaryExpression>(
                "Convert trash",
                z => z.NodeType==ExpressionType.Convert, 
                z => z.Operand, 
                z => null,
                false);

            yield return new ChopRule<MemberExpression>(
                "Fields and Props",
                z => z.NodeType==ExpressionType.MemberAccess, 
                z => z.Expression, 
                z => z.Member.Name,
                false);

            yield return new ChopRule<MethodCallExpression>(
                "List indexation",
                z=>z.NodeType==ExpressionType.Call && z.Method.Name.StartsWith("get_Item") && z.Arguments[0] is ConstantExpression,
                z=>z.Object,
                z=> { var val=((ConstantExpression)z.Arguments[0]).Value; return TypeFormat.GetDefaultFormat(val.GetType()).WriteObject(val); },
                false);

            yield return new ChopRule<ParameterExpression>(
                "End",
                z => true,
                z => z,
                z => null,
                true);

          
        }


        static List<IChopRule> rules;
        static ExpressionToAddressConverter()
        {
            rules=GetRules().ToList();
        }

        public static FieldAddress GetAddress<TIn,TOut>(Expression<Func<TIn, TOut>> address) { return GetAddress(address.Body); }

        public static FieldAddress GetAddress(Expression e)
        {
            var a=new List<string>();
            while (true)
            {
                bool stop = false;
                bool ok=false;
                foreach(var r in rules)
                    if (r.Make(ref e, a, ref stop)) { ok = true; break; }
                if (!ok)
                    throw new Exception("Can't resolve expression to field address. Perhaps, it is NOT field address?");
                if (stop) break;
            }
            var res = FieldAddress.Root;
            a.Reverse();
            foreach (var v in a)
                res = res.Child(v);
            return res;
        }
       



    }
    
}
