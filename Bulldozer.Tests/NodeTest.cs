using System;
using System.Linq;
using AIRLab.CA;
using NUnit.Framework;

namespace BulldozerTest
{
    [TestFixture]
    public class NodeTest
    {
        [Test]
        public void TreeHasCorrectStringForm()
        {
            var tree = new Arithmetic.Product<int>(VariableNode.Make<int>(0, "x"),
                                                   new Arithmetic.Plus<int>(VariableNode.Make<int>(1, "y"),
                                                                            Constant.Int(3)));
            Assert.AreEqual(tree.ToString(), "(x ∙ (y + 3))");
        }

        [Test]
        public void NodesKeepProperInfo()
        {
            var constant = Constant.Int(1);
            Assert.AreEqual(1, constant.Value);
            Assert.AreEqual(typeof (int), constant.Type);
            Assert.AreEqual(0, constant.Children.Length);

            var variable = VariableNode.Make<int>(0, "x");
            Assert.AreEqual(0, variable.Index);
            Assert.AreEqual(typeof (int), variable.Type);
            Assert.AreEqual(0, variable.Children.Length);

            var op = new Arithmetic.Plus<int>(constant, variable);
            Assert.AreEqual(op.Children.Length, 2);
            Assert.AreEqual(op.Type, typeof (int));
            Assert.AreEqual(op.Parent, null);
            Assert.AreEqual(variable.Parent, op);
            Assert.AreEqual(constant.Parent, op);
        }

        [Test]
        public void Fun()
        {
            var table = new[]
                            {
                                new double[] {0, 1, 2},
                                new double[] {0, 1, 2, 3},
                                new double[] {0, 1}
                            };
            var vector = table.Select(l => l.First()).ToArray();

            Enumerate(vector, table, 0);

//            var idx = vector.Length - 3;
//
//            foreach (var v1 in table[idx])
//            {
//                vector[idx] = v1;
//                foreach (var v2 in table[idx + 1])
//                {
//                    vector[idx + 1] = v2;
//                    foreach (var v3 in table[idx + 2])
//                    {
//                        vector[idx + 2] = v3;
//                        WriteVector(vector);
//                    }
//                }
//            }
        }

        private static void Enumerate(double[] vector, double[][] table, int idx)
        {
            foreach (var value in table[idx])
            {
                vector[idx] = value;
                if(idx == vector.Length - 1)
                {
                    WriteVector(vector);
                    continue;
                }
                Enumerate(vector, table, idx + 1);
            }
        }

        private static void WriteVector(double[] vector)
        {
            Console.WriteLine(vector.Aggregate(string.Empty, (seed, e) => seed + " " + e));
        }
    }
}