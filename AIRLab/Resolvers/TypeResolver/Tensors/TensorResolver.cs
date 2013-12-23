using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace AIRLab.Thornado
{
   
      
    enum TensorResolverType
    {
        LinearArray,
        TensorArray,
        Intermediate,
        Final

    }

    public class TensorResolver<T,A> : TypeResolver
    {
        readonly TensorResolverType TRType;
        readonly int rank;

        public TensorResolver()
        {
            if (typeof(A).IsArray)
            {
                rank=typeof(A).GetArrayRank();
                if (rank==1)
                    TRType= TensorResolverType.LinearArray;
                else TRType = TensorResolverType.TensorArray;
            }
            else 
            {
                rank=TensorIntermediateType<T>.TITIndex(typeof(A));
                if (rank == -1)
                    throw new Exception("Неожиданный генерик аргумент TensorResolver");
                if (rank ==1)
                    TRType = TensorResolverType.Final;
                else
                    TRType = TensorResolverType.Intermediate;
            }
        }

        public override IEnumerable<string> GetSubaddresses(object obj)
        {
            Array array = null;
            int rank = -1;

            switch (TRType)
            {
                case TensorResolverType.TensorArray:
                case TensorResolverType.LinearArray:
                    array = (Array)obj;
                    rank = 0;
                    break;
                case TensorResolverType.Intermediate:
                case TensorResolverType.Final:
                    var tit = (TensorIntermediateType<T>)obj;
                    array = tit.CorrespondingArray;
                    rank = tit.Position;
                    break;
            }
            
            foreach (var str in Enumerable.Range(0, array.GetLength(rank)))
                yield return str.ToString();

        }

        public override object GetElement(object obj, string sub)
        {
            int index = int.Parse(sub);
            switch(TRType)
            {
                case TensorResolverType.LinearArray:
                    return ((Array)obj).GetValue(index);
                case TensorResolverType.TensorArray:
                    return TensorIntermediateType<T>.First((Array)obj,index);
                case TensorResolverType.Intermediate:
                    return (obj as TensorIntermediateType<T>).Next(index);
                case TensorResolverType.Final:
                    var tit = obj as TensorIntermediateType<T>;
                    return tit.CorrespondingArray.GetValue(tit.Resolve(index));
            }
            throw new Exception("");
        }


        public override void SetElement(object obj, string sub, object value)
        {
            int index=int.Parse(sub);
            switch (TRType)
            {
                case TensorResolverType.LinearArray:
                    (obj as Array).SetValue(value, index);
                    break;
                case TensorResolverType.Final:
                    var tit = obj as TensorIntermediateType<T>;
                    tit.CorrespondingArray.SetValue(value, tit.Resolve(index));
                    break;
                default:
                    throw new Exception("Cannot set intermediate level of tensor");
            }
            
        }

      
        public override IEnumerable<string> GetDefinedSubaddresses()
        {
            yield return "*";
        }

        public override Type GetDefinedType(string sub)
        {
            switch (TRType)
            {
                case TensorResolverType.LinearArray:
                case TensorResolverType.Final:
                    return typeof(T);
                case TensorResolverType.TensorArray:
                case TensorResolverType.Intermediate:
                    return TensorIntermediateType<T>.GetTITType(rank - 1);
            }
            throw new Exception("");
        }

       
        List<object> GetMoldArray(IEnumerable<KeyValuePair<string,object>> mold, object defaultValue, LogicErrorList errors)
        {
            var list = new List<object>();
            foreach(var e in mold)
            {
                int index=0;
                try
                {
                    index=int.Parse(e.Key);
                }
                catch
                {
                    errors.Add(LogicErrorLevel.Error,"Неверный адрес массива "+e.Key);
                    continue;
                }
                while(list.Count<=index) list.Add(defaultValue);
                list[index]=e.Value;
            }
            return list;
        }

        void GetMaxIndices( IList parsed, int[] lengths, int level)
        {
            lengths[level] = Math.Max(lengths[level], parsed.Count);
            if (level < lengths.Length - 1)
                for (int i = 0; i < parsed.Count; i++)
                    GetMaxIndices((IList)parsed[i], lengths, level + 1);
        }

        void CopyToArray(Array array, IList parsed, int[] arguments, int level)
        {
            for (int i = 0; i < parsed.Count; i++)
            {
                arguments[level]=i;
                if (level < arguments.Length - 1)
                    CopyToArray(array, (IList)parsed[i], arguments, level + 1);
                else
                    array.SetValue(parsed[i], arguments);
            }

        }

        public override object Create(IEnumerable<KeyValuePair<string, object>> mold, LogicErrorList list)
        {
            switch (TRType)
            {
                case TensorResolverType.LinearArray:
                    return GetMoldArray(mold, default(T), list).Cast<T>().ToArray();
                case TensorResolverType.TensorArray:
                    var args = new int[rank];
                    var ls = mold.Select(z => z.Value).ToList();
                    GetMaxIndices(ls, args, 0);
                    var ar = Array.CreateInstance(typeof(T), args);
                    CopyToArray(ar, ls, args, 0);
                    return ar;
                case TensorResolverType.Final:
                case TensorResolverType.Intermediate:
                    return GetMoldArray(mold,
                        TRType == TensorResolverType.Final ? (object)default(T) : null,
                        list);
            }
            throw new Exception("");
        }

    }
}
