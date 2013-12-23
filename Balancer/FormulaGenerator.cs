using System;
using AIRLab.Bulldozer;
using System.Linq;
using AIRLab.CA;

namespace Balancer
{
    public static class FormulaGenerator
    {
        private static readonly Random Rnd = new Random();
        private static readonly Type[] Operations;
        private static readonly string[] VariablesNames = "xyzuv".Select(c => c.ToString()).ToArray();
        private const int ConstantMax = 5;

        static FormulaGenerator()
        {
            Operations = typeof(Arithmetic).GetNestedTypes()
                .Where(t => t.IsGenericType && t.IsSubclassOf(typeof(BinaryOp)))
                .ToArray();
        }

        public static string Generate(int operationsCount, int variablesCount)
        {
            var constantsCount = operationsCount - variablesCount + 1;

            if(constantsCount < 0) throw new ArgumentException("Wrong operations-variables relation");

            var variables = Enumerable.Range(0, variablesCount)
                .Select(i => VariableNode.Make<double>(i, VariablesNames[i]))
                .Cast<INode>()
                .ToList();

            var constants = Enumerable.Range(0, constantsCount)
                .Select(i => Constant.Double(ConstantMax))
                .Cast<INode>()
                .ToList();

            var leafs = constants.Union(variables).ToList();

            while (leafs.Count != 1)
            {
                var first = leafs.First();
                var last = leafs.Last();

                leafs.Add(GetRandomBinaryOperation(first, last));

                leafs.Remove(first);
                leafs.Remove(last);
            }
            return leafs.Single().ToPrefixForm();
        }

        private static INode GetRandomBinaryOperation(INode child1, INode child2)
        {
            var idx = Rnd.Next(Operations.Length);
            var op = Operations[idx].MakeGenericType(typeof (double))
                                    .GetConstructors()
                                    .First()
                                    .Invoke(new object[]{child1, child2});
            return op as INode;
        }
    }
}