using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using AIRLab.Bulldozer;
using AIRLab.CA;
using AIRLab.CA.Tools;
using AIRLab.Thornado;
using Balancer;
using NUnit.Framework;
using System.IO;

namespace TaskGenerator
{
    [TestFixture]
    public class TestClass
    {
        //        [Test]
        //        public void Translate()
        //        {
        //            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");
        //            var lines = File.ReadAllLines(@"C:\Users\RaZiel\Desktop\FormulaSet.txt");
        //            var result = lines.Select(line => "\"" + String2Tree.Parse(line).ToPrefixForm() + "\"");
        //            var array = string.Join(",\r\n", result);
        //            Console.WriteLine(array);
        //        }

        [Test]
        public void TestMethod()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");
            ForEachExtension.ForEach(Directory.GetFiles(".")
                                  .Where(e => e.Contains(".ini")), File.Delete);
            CreateSettings("Settings.ini");
            TaskGenerator.Main(new[] { "Settings.ini" });
            //            Balancer.Balancer.Main(new[] {"Task 2.ini", "Point 0,1 1 1 1 1 0,6.ini"});
        }

        private static void CreateSettings(string fileName)
        {
            var settings = new Settings
            {
                Tasks = new[] {
//                                        "Divide(Product(Minus(Constant(0),Constant(1)),VariableNode(0|x)),Minus(Constant(2),VariableNode(0|x)))",
//                                        "Pow(Pow(VariableNode(0|x),Constant(0.5)),VariableNode(0|x))",
//                                        "Pow(Pow(VariableNode(0|x),VariableNode(0|x)),Constant(0.5))",
//                                        "Divide(Plus(VariableNode(0|x),VariableNode(0|x)),Constant(3))",
//                                        "Divide(VariableNode(0|x),Plus(Constant(2),VariableNode(0|x)))",
//                                        "Pow(VariableNode(0|x),Divide(VariableNode(0|x),Constant(3)))",
//                                        "Pow(Pow(VariableNode(0|x),VariableNode(0|x)),Constant(0.5))",
//                                        "Divide(Minus(Constant(0),VariableNode(0|x)),Minus(Constant(3),VariableNode(0|x)))",
//                                        "Divide(VariableNode(0|x),Plus(Minus(Constant(0),Constant(3)),VariableNode(0|x)))",
//                                        "Plus(VariableNode(0|x),Divide(VariableNode(0|x),Constant(3)))",
//                                        "Pow(Product(VariableNode(0|x),VariableNode(0|x)),Divide(VariableNode(0|x),Constant(6)))",
//                                        "Minus(VariableNode(0|x),Pow(VariableNode(0|x),VariableNode(1|y)))",
//                                        "Minus(VariableNode(1|y),Pow(VariableNode(1|y),VariableNode(0|x)))",
//                                        "Minus(Minus(Product(VariableNode(0|x),VariableNode(0|x)),Constant(4)),VariableNode(0|x))",
//                                        "Minus(VariableNode(0|x),Minus(Constant(2),Pow(VariableNode(0|x),Constant(3))))",
//                                        "Product(Product(VariableNode(0|x),VariableNode(0|x)),Product(VariableNode(0|x),Plus(Plus(Product(VariableNode(0|x),VariableNode(0|x)),VariableNode(0|x)),VariableNode(0|x))))",
//                                        "Product(Plus(Minus(Constant(0),Constant(3)),Product(VariableNode(0|x),VariableNode(0|x))),VariableNode(0|x))",
//                                        "Pow(Product(VariableNode(0|x),Product(VariableNode(0|x),Constant(9))),VariableNode(0|x))",
//                                        "Product(Product(VariableNode(0|x),Product(Plus(Constant(4),VariableNode(0|x)),VariableNode(0|x))),VariableNode(0|x))",
//                                        "Divide(Minus(Constant(0),Constant(8)),Divide(Constant(3),VariableNode(0|x)))",
                                        "Product(VariableNode(0|x),Plus(Constant(4),Product(VariableNode(0|x),VariableNode(0|x))))",
//                                        "Minus(Pow(VariableNode(0|x),Constant(3)),Minus(Constant(4),VariableNode(0|x)))",
//                                        "Plus(Plus(Minus(Constant(0),Constant(2)),VariableNode(0|x)),Divide(VariableNode(0|x),Constant(4)))",
//                                        "Product(VariableNode(0|x),Plus(Plus(Minus(Constant(0),Constant(6)),Plus(VariableNode(0|x),VariableNode(0|x))),VariableNode(0|x)))",
//                                        "Product(Minus(Constant(4),VariableNode(0|x)),Minus(Constant(2),VariableNode(0|x)))",
//                                        "Plus(Minus(Constant(3),VariableNode(0|x)),Divide(VariableNode(0|x),Constant(4)))",
//                                        "Plus(Minus(Constant(0),Constant(16)),Product(VariableNode(0|x),VariableNode(0|x)))",
//                                        "Plus(Constant(2),Product(VariableNode(0|x),Constant(5)))",
//                                        "Minus(Constant(3),Minus(VariableNode(0|x),Pow(VariableNode(0|x),Constant(4))))",
//                                        "Product(Plus(Plus(Minus(Constant(0),Constant(6)),VariableNode(0|x)),VariableNode(0|x)),VariableNode(0|x))",
//                                        "Product(Minus(Constant(0),Constant(2)),Plus(Constant(1),Divide(Divide(VariableNode(0|x),Minus(Constant(0),Constant(0.5))),Minus(Constant(0),Constant(6)))))",
//                                        "Divide(Pow(VariableNode(0|x),Constant(4)),Constant(4))",
//                                        "Pow(VariableNode(0|x),Product(Pow(VariableNode(0|x),Constant(3)),Constant(3)))",
//                                        "Product(Product(Constant(4),Minus(Constant(3),Minus(Constant(0),VariableNode(0|x)))),VariableNode(0|x))",
//                                        "Plus(Plus(Minus(Constant(0),Constant(6)),VariableNode(0|x)),Product(VariableNode(0|x),VariableNode(0|x)))",
//                                        "Product(Product(Minus(Constant(0),Constant(4)),Plus(Minus(Constant(0),Constant(5)),Minus(Constant(2),VariableNode(0|x)))),VariableNode(0|x))",
//                                        "Plus(Plus(Minus(Constant(0),Constant(8)),Plus(VariableNode(0|x),Pow(VariableNode(0|x),Constant(2)))),VariableNode(0|x))",
//                                        "Plus(Plus(Minus(Constant(0),Constant(2)),VariableNode(0|x)),Pow(VariableNode(0|x),Constant(4)))",
//                                        "Minus(Plus(Minus(Constant(0),Constant(2)),VariableNode(0|x)),Divide(VariableNode(0|x),Minus(Constant(0),Constant(4))))",
////                                        "Product(Product(VariableNode(0|x),Plus(Constant(4),VariableNode(0|x))),VariableNode(0|x))",
//                                        "Divide(Pow(VariableNode(0|x),Constant(5)),Constant(2))",
//                                        "Plus(Plus(Constant(4),VariableNode(0|x)),Product(Product(Minus(Constant(0),VariableNode(0|x)),VariableNode(0|x)),VariableNode(0|x)))",
//                                        "Product(Divide(VariableNode(0|x),Plus(Minus(Constant(0),Constant(2)),VariableNode(0|x))),VariableNode(0|x))",
//                                        "Minus(Constant(2),Divide(VariableNode(0|x),Constant(2)))",
//                                        "Minus(Product(Minus(Constant(0),Constant(3)),Minus(Constant(1),VariableNode(1|y))),VariableNode(0|x))",
//                                        "Divide(Plus(Product(Minus(Constant(0),Constant(2)),VariableNode(0|x)),Product(VariableNode(1|y),VariableNode(0|x))),Constant(3))",
//                                        "Minus(Product(Divide(VariableNode(0|x),Constant(3)),VariableNode(1|y)),VariableNode(1|y))",
//                                        "Product(Product(VariableNode(1|y),VariableNode(1|y)),Plus(Constant(2),VariableNode(0|x)))",
//                                        "Plus(Plus(Plus(Plus(Minus(Constant(0),Constant(3)),VariableNode(0|x)),VariableNode(1|y)),VariableNode(0|x)),VariableNode(0|x))",
//                                        "Product(VariableNode(0|x),Product(Plus(Constant(3),VariableNode(1|y)),VariableNode(0|x)))",
//                                        "Pow(Product(VariableNode(1|y),Product(Constant(9),VariableNode(1|y))),VariableNode(0|x))",
//                                        "Plus(Plus(VariableNode(0|x),VariableNode(0|x)),Product(VariableNode(1|y),VariableNode(1|y)))",
//                                        "Product(Plus(Constant(3),VariableNode(0|x)),Plus(VariableNode(1|y),VariableNode(1|y)))",
//                                        "Plus(Plus(Plus(Plus(VariableNode(0|x),VariableNode(1|y)),VariableNode(0|x)),Constant(3)),VariableNode(0|x))",
//                                        "Plus(Plus(VariableNode(0|x),VariableNode(0|x)),Divide(VariableNode(1|y),Constant(2)))",
//                                        "Plus(VariableNode(1|y),Product(Divide(VariableNode(0|x),Constant(3)),VariableNode(1|y)))",
//                                        "Product(Product(VariableNode(1|y),Plus(Constant(2),VariableNode(0|x))),VariableNode(1|y))",
//                                        "Minus(Minus(Constant(0),Constant(2)),Minus(Product(VariableNode(1|y),VariableNode(1|y)),VariableNode(0|x)))",
//                                        "Divide(Pow(VariableNode(0|x),Constant(5)),Constant(4))",
//                                        "Plus(Divide(VariableNode(0|x),Minus(Constant(3),VariableNode(0|x))),VariableNode(0|x))",
//                                        "Plus(Product(VariableNode(0|x),VariableNode(0|x)),Plus(Plus(Minus(Constant(0),Constant(4)),VariableNode(0|x)),VariableNode(0|x)))",
//                                        "Product(Product(Plus(Divide(VariableNode(0|x),Pow(Constant(1),VariableNode(0|x))),VariableNode(0|x)),Plus(Constant(5),VariableNode(0|x))),VariableNode(0|x))",
//                                        "Divide(Product(VariableNode(0|x),Minus(Constant(1),VariableNode(0|x))),Plus(VariableNode(0|x),Constant(3)))",
//                                        "Divide(Minus(Constant(0),Constant(3)),Minus(Constant(3),VariableNode(0|x)))",
//                                        "Minus(Product(Constant(3),VariableNode(0|x)),Minus(Minus(Constant(0),Constant(2)),VariableNode(0|x)))",
//                                        "Product(Plus(Constant(5),VariableNode(0|x)),Plus(Constant(1),VariableNode(0|x)))",
//                                        "Product(Minus(Constant(1),VariableNode(0|x)),Minus(Constant(3),VariableNode(0|x)))",
//                                        "Pow(Pow(VariableNode(0|x),Product(Product(VariableNode(0|x),VariableNode(0|x)),VariableNode(0|x))),Constant(4))",
//                                        "Plus(Minus(Constant(0),Constant(5)),Pow(VariableNode(0|x),Constant(3)))",
//                                        "Pow(Pow(VariableNode(0|x),Constant(9)),Plus(VariableNode(0|x),Constant(3)))",
//                                        "Minus(VariableNode(0|x),Divide(VariableNode(0|x),Constant(5)))",
//                                        "Divide(VariableNode(0|x),Plus(Plus(Minus(Constant(0),Constant(8)),VariableNode(1|y)),VariableNode(1|y)))",
//                                        "Product(Product(Divide(Product(VariableNode(0|x),VariableNode(0|x)),Constant(3)),VariableNode(1|y)),VariableNode(1|y))"
                                   },
                Metrics = new[] {
                                                     new Parameter
                                                         {
                                                             MaxValue = 5,
                                                             MinValue = 0,
                                                             Names = new[]{ "Length" },
                                                             PointCount = 11,
                                                             GenerationType = GenerationType.Linear,
                                                             DefaulValue = 1,
                                                             Sequence = new[]
                                                                            {
                                                                                0,
                                                                                0.01,
                                                                                1
                                                                            }
                                                         }
                                                   },
                SetWeights = new[] {
                                                      new Parameter
                                                      {
                                                          MaxValue = 1,
                                                          MinValue = 1,
                                                          Names = new[]{ StdTags.SafeBlowing },
                                                          PointCount = 0,
                                                          GenerationType = GenerationType.Constant,
                                                          DefaulValue = 1
                                                      },
                                                      new Parameter
                                                      {
                                                          MaxValue = 1,
                                                          MinValue = 1,
                                                          Names = new[]{ StdTags.UnsafeBlowing },
                                                          PointCount = 0,
                                                          GenerationType = GenerationType.Constant,
                                                          DefaulValue = 1
                                                      },
                                                      new Parameter
                                                      {
                                                          MaxValue = 1,
                                                          MinValue = 1,
                                                          Names = new[]{ StdTags.Tunning },
                                                          PointCount = 0,
                                                          GenerationType = GenerationType.Constant,
                                                          DefaulValue = 1
                                                      },
                                                      new Parameter
                                                      {
                                                          MaxValue = 0.5,
                                                          MinValue = 0,
                                                          Names = new[]{ StdTags.SafeResection },
                                                          PointCount = 11,
                                                          GenerationType = GenerationType.Constant,
                                                          Sequence = new[]
                                                                         {
                                                                             0,
                                                                             0.3,
                                                                             2
                                                                         },
                                                          DefaulValue = 0
                                                      },
                                                      new Parameter
                                                      {
                                                          MaxValue = 1,
                                                          MinValue = 0,
                                                          Names = new[]{ StdTags.UnsafeResection },
                                                          PointCount = 11,
                                                          GenerationType = GenerationType.Linear,
                                                          Sequence = new[]
                                                                         {
                                                                             0,
                                                                             0.3,
                                                                             2
                                                                         },
                                                          DefaulValue = 0
                                                      }
                                                },
                SampleMinValue = 0,
                SampleMaxValue = 10,
                SampleDelta = 1,
                SampleNoise = 0.1,
                AlgorithmRunsCount = 60,
                IterationCount = 500,
                Threshold = 0.95,
                OnlineSimplification = false,
                OnlineSimplificationRate = 0
            };
            IO.INI.WriteToFile(fileName, settings);
        }
    }
}