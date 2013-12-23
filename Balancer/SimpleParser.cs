using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using AIRLab.Bulldozer;
using AIRLab.CA;

namespace Balancer
{
    public class SimpleParser<T, TN>
    {
        private readonly Func<string, TN> _underliedTypeParser;
        private readonly Type[] _operations;
        private readonly string[] _names;

        public SimpleParser(Func<string, TN> underliedTypeParser, Func<Type, bool> typeSelector)
        {
            _underliedTypeParser = underliedTypeParser;
            _operations = typeof(T).GetNestedTypes()
                .Where(typeSelector)
                .ToArray();
            _names = _operations.Select(t => t.CleanTypeName()).ToArray();
        }

        public INode Parse(string formula)
        {
            var trimedFormula = Regex.Replace(formula, @"\s", "");
            var pair = CutOffHead(trimedFormula);
            return BuildTree(pair.Item1, pair.Item2);
        }

        private INode BuildTree(string parent, string[] children)
        {
            var idx = _names.IndexOf(parent);
            if (idx == -1) return BuildLeaf(parent, children.Single());
            var parameters = children.Select(CutOffHead)
                                     .Select(p => BuildTree(p.Item1, p.Item2))
                                     .Cast<object>()
                                     .ToArray();
            return InvokeConstructor(_operations[idx], parameters);
        }

        private static INode InvokeConstructor(Type type, object[] parameters)
        {
            ConstructorInfo constructor;
            if (type.IsGenericType)
                constructor = type.MakeGenericType(typeof(TN)).GetConstructors().First();
            else
            {
                constructor = type.GetConstructors().First();
                parameters = new object[] {typeof (TN)}.Union(parameters).ToArray();
            }
            return constructor.Invoke(parameters) as INode;
        }

        private INode BuildLeaf(string parent, string innerData)
        {
            if (parent == "Constant") return new Constant<TN>(_underliedTypeParser(innerData));
            if (parent == "VariableNode")
            {
                var data = innerData.Split('|');
                return VariableNode.Make<TN>(int.Parse(data[0]), data[1]);
            }
            throw new ArgumentException("Wrong token");
        }

        private static Tuple<string, string[]> CutOffHead(string body)
        {
            var match = Regex.Match(body, @"^(.+?)\((.+)\)$");
            var head = match.Groups[1].Value;
            var tail = match.Groups[2].Value;
            return new Tuple<string, string[]>(head, SplitTail(tail));
        }

        private static string[] SplitTail(string tail)
        {
            var parts = new List<string>();
            var counter = 0;
            var startPosition = 0;
            var i = 0;
            for (; i < tail.Length; ++i)
            {
                if (tail[i] == '(') ++counter;
                if (tail[i] == ')') --counter;
                if (tail[i] != ',' || counter != 0) continue;
                parts.Add(tail.Substring(startPosition, i - startPosition));
                startPosition = i + 1;
            }
            parts.Add(tail.Substring(startPosition));
            return parts.ToArray();
        }
    }
}
