using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskGenerator;
using AIRLab.CA;
using AIRLab.Thornado;
using System.IO;

namespace Balancer.Test
{
    public class DebugRunner
    {
        public static void Main()
        {/*
			 foreach (var e in Directory.GetFiles("."))
                if (e.Contains(".ini")) File.Delete(e);
           
            CreateSettings("Settings.ini");
            TaskGenerator.TaskGenerator.Main(new[] { "Settings.ini" });
            Balancer.ShowTrace = true;
            Balancer.Main(new[] {"Task 1.ini", "Point 0_5 1 1 1 1 0_2.ini"});
        }

        private static void CreateSettings(string fileName)
        {
            var settings = new Settings
            {
                Tasks = new[] {"Plus(Constant(5),VariableNode(0|x))"},
                Metrics = new[] {
                                                     new Parameter
                                                         {
                                                             MaxValue = 0.3,
                                                             MinValue = 0.2,
                                                             Name = "Length",
//                                                             Step = 0.1
                                                         }
                                                   },
                SetWeights = new[] {
                                                  new Parameter
                                                      {
                                                          MaxValue = 0.6,
                                                          MinValue = 0.5,
                                                          Name = StdTags.Inductive,
//                                                          Step = 0.1
                                                      },
                                                      new Parameter
                                                      {
                                                          MaxValue = 1,
                                                          MinValue = 1,
                                                          Name = StdTags.Algebraic,
//                                                          Step = 0.1
                                                      },
                                                      new Parameter
                                                      {
                                                          MaxValue = 1,
                                                          MinValue = 1,
                                                          Name = StdTags.SafeBlowing,
//                                                          Step = 0.1
                                                      },
                                                      new Parameter
                                                      {
                                                          MaxValue = 1,
                                                          MinValue = 1,
                                                          Name = StdTags.SafeResection,
//                                                          Step = 0.1
                                                      },
                                                      new Parameter
                                                      {
                                                          MaxValue=1,
                                                          MinValue=1,
                                                          Name=StdTags.Tunning,
//                                                          Step = 0.1
                                                      }

                                                },
                SampleMinValue = 0,
                SampleMaxValue = 10,
                SampleDelta = 1,
                SampleNoise = 0.1,
                AlgorithmRunsCount = 10,
                IterationCount = 20000,
                Threshold = 0.95
            };
            IO.INI.WriteToFile(fileName, settings);*/
        }
    }
}