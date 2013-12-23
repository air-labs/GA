using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;

namespace AIRLab.Thornado
{
    public class ClassResolver<T> : TypeResolver
    {

        #region Статический сахарок 

        public static Q GetAD<Q>(Expression<Func<T, object>> address, ADSpecification<Q> spec)
        {
            var res = TypeResolver.GetResolver(typeof(T)) as ClassResolver<T>;
            return res.TypeAD.Get(ExpressionToAddressConverter.GetAddress(address.Body), spec);
        }

        public static void SetAD<Q>(Expression<Func<T, object>> address, ADSpecification<Q> spec, Q value)
        {
            var res = TypeResolver.GetResolver(typeof(T)) as ClassResolver<T>;
            res.TypeAD.Set(ExpressionToAddressConverter.GetAddress(address.Body), spec, value);
        }

        public static void SetAD(Expression<Func<T, object>> address, ADDefinition def)
        {
            var res = TypeResolver.GetResolver(typeof(T)) as ClassResolver<T>;
            res.TypeAD.Set(ExpressionToAddressConverter.GetAddress(address.Body), def);
        }

        public static void OverrideFields(params string[] fieldNames)
        {
            var res = TypeResolver.GetResolver(typeof(T)) as ClassResolver<T>;
            res.fields = new Dictionary<string, FieldScan>();
            var type = typeof(T);
            foreach (var e in fieldNames)
            {
                if (type.GetProperty(e) != null)
                    res.fields[e] = new FieldScan(type.GetProperty(e));
                else if (type.GetField(e) != null)
                    res.fields[e] = new FieldScan(type.GetField(e));
            }
        }

        public static void OverrideConstructors(params Delegate[] delegates)
        {
            var res = TypeResolver.GetResolver(typeof(T)) as ClassResolver<T>;
            res.constructors.Clear();
            foreach (var d in delegates)
                res.constructors.Add(new ConstructorData(d));
        }


        #endregion

        #region Инициалзация
        /// <summary>
        /// Здесь храняться поля/свойства со значениями Значимых атрибутов
        /// </summary>
        Dictionary<string, FieldScan> fields;
        

        List<ConstructorData> constructors;
       
        /// <summary>
        /// Заполнение информации о полях/свойствах класса
        /// </summary>
       public  ClassResolver()
        {
            List<MemberInfo> mems = new List<MemberInfo>();
            Type check = typeof(T);
            while (true)
            {
                if (check == typeof(object)) break;
                mems.AddRange(check.GetMembers(BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance));
                check = check.BaseType;
                
            }
            fields = new Dictionary<string, FieldScan>();
            foreach (var e in mems.Where(z => z is PropertyInfo || z is FieldInfo))
            {
                var attrs = e.GetCustomAttributes(typeof(IADAttribute), false).OrderBy(z => z.GetType().ToString()).Cast<IADAttribute>().ToArray();
                if (attrs.Count() == 0) continue;
                var scan=new FieldScan(e);
                fields[scan.FieldName] = scan;
                foreach (var a in attrs)
                    foreach (var d in a.DefineAD(scan.Type))
                        TypeAD.Set(new FieldAddress(scan.FieldName), d);
            }

          
            

            constructors = new List<ConstructorData>();
            foreach (var c in typeof(T).GetConstructors())
                constructors.Add(new ConstructorData(c));
            constructors = constructors.OrderBy(z => -z.ParamCount).ToList();
        }

        
        #endregion
      
        public override void SetElement(object obj, string sub, object value)
        {
            fields[sub].Set(obj, value);
        }

        public override IEnumerable<string> GetDefinedSubaddresses()
        {
            return fields.Keys;
        }

        public override Type GetDefinedType(string sub)
        {
            return fields[sub].Type;
        }

        

        public override IEnumerable<string> GetSubaddresses(object obj)
        {
            return fields.Keys;
        }

        public override object GetElement(object obj, string sub)
        {
            return fields[sub].Get(obj);
        }

        public override object Create(IEnumerable<KeyValuePair<string, object>> mold, LogicErrorList list)
        {
            var invs = constructors.Select(z => z.CreateInvocation()).ToList();

            foreach (var e in mold)
                foreach (var i in invs)
                    i.MatchParam(e.Key, e.Value);

            var c = invs.Where(z => z.Ready).FirstOrDefault();
            if (c == null)
                list.Add(LogicErrorLevel.Error, "Невозможно создать объект: не удается вызвать ни один конструктор");
            
            var obj = c.Invoke();

            mold = mold.Where(z => !c.ContructorData.ParamNames.Contains(z.Key)).ToList();

            foreach (var e in mold)
                if (!fields.ContainsKey(e.Key))
                {
                    list.Add(LogicErrorLevel.Error, "Класс " + typeof(T) + " не содержит поля " + e);
                    continue;
                }
                else
                    SetElementAndProcessErrors(obj, e.Key, e.Value, list);

            return obj;
        }

    }
}
