
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AIRLab.Bulldozer;
using AIRLab.CA;
using AIRLab.Thornado;
using Balancer;

namespace TaskGenerator
{
    public static class TaskGenerator
    {
        private const string TaskName = "Task {0}.ini";
        private const string PointName = "Point {0}.ini";
        private const string BaseFormat = 
@"#!/bin/bash

case ""$PMI_RANK"" in
 {0}
esac";
        private const string ItemFormat = 
@"{0})
{1} ""{2}"" ""{3}""
;;";

        private const string BalancerExe = "/opt/mono-2.10/bin/mono Balancer.exe";
        private static readonly List<string> Tasks = new List<string>();
        private static readonly List<string> Points = new List<string>();

        public static void Main(string[] args)
        {
            if(args.Length != 1)
            {
                Console.WriteLine("Usage: TaskGenerator.exe settings.ini");
                return;
            }
            var settings = IO.INI.ParseFile<Settings>(args[0]);
            TasksGeneration(settings);
            PointsGeneration(settings);
//            CreateRunner();
            CreateWinRunner();
        }

        private static void CreateWinRunner()
        {
            const string format = @"Balancer.exe ""{0}"" ""{1}""";
            var items = from task in Tasks
                        from point in Points
                        select string.Format(format, task, point);
            File.WriteAllText("run.bat", items.Aggregate(string.Empty, (str, s) => str + "\r\n" + s));
        }

        private static void CreateRunner()
        {
            var items = string.Empty;
            var i = 0;
            foreach (var task in Tasks)
            {
                foreach (var point in Points)
                {
                    var item = string.Format(ItemFormat, i, BalancerExe, task, point);
                    items = items + "\r\n" + item;
                    ++i;
                }
            }
            File.WriteAllText("run.sh", string.Format(BaseFormat, items));
        }

        private static void TasksGeneration(Settings settings)
        {
            settings.Tasks.Select((t, i) => new {
                                                    Task = GenerateTask(t, settings),
                                                    Idx = i + 1
                                                })
                           .ForEach(t =>
                                        {
                                            var name = string.Format(TaskName, t.Idx);
                                            IO.INI.WriteToFile(name, t.Task);
                                            Tasks.Add(name);
                                        });
        }

        private static void PointsGeneration(Settings settings)
        {
            var tagsTable = settings.SetWeights.Select(GenerateRow);

            var metricsTable = settings.Metrics.Select(GenerateRow).ToArray();

            var table = tagsTable.Union(metricsTable).ToArray();
            var vector = table.Select(l => l.First()).ToArray();
            Enumerate(vector, table, 0, settings);
        }

        private static double[] GenerateRow(Parameter parameter)
        {
            if (parameter.GenerationType == GenerationType.Constant)
            {
                return new[] {parameter.DefaulValue};
            }
            if(parameter.GenerationType == GenerationType.Sequence)
            {
                return parameter.Sequence;
            }
            return Enumerable.Range(0, parameter.PointCount)
                             .Select(n => (double)n / (parameter.PointCount - 1))
                             .Select(n => Transfer(n, parameter))
                             .ToArray();
        }

        private static double Transfer(double num, Parameter parameter)
        {
            var max = parameter.MaxValue;
            var min = parameter.MinValue;
            if(parameter.GenerationType == GenerationType.Linear)
            {
                return min + num * (max - min);
            }
            if(parameter.GenerationType == GenerationType.Exponental)
            {
                return min * Math.Pow(max/min, num);
            }
            throw new ArgumentException("Тип генерации не поддерживается");
        }

        private static void Enumerate(double[] vector, double[][] table, int idx, Settings settings)
        {
            foreach (var value in table[idx])
            {
                vector[idx] = value;
                if (idx == vector.Length - 1)
                {
                    ApplyVector(vector, settings.SetWeights, settings.Metrics);
                    continue;
                }
                Enumerate(vector, table, idx + 1, settings);
            }
        }

        //TODO: переписать гомосятину
        private static void ApplyVector(double[] vector, Parameter[] parameters, Parameter[] metrics)
        {
            var tags = new Dictionary<string, double>();
            var ii = 0;
            foreach (var parameter in parameters)
            {
                ++ii;
                foreach (var name in parameter.Names)
                {
                    var idx = parameters.IndexOf(parameter);
                    tags.Add(name, vector[idx]);
                }
            }
            var point = new Point
            {
                Tags = tags,
                Metrics = metrics.Select((m, i) => new { Name = m.Names.First(), Idx = i + ii })
                           .ToDictionary(p => p.Name, p => vector[p.Idx])
            };
            var vectorName = WriteVector(vector);
            var fileName = string.Format(PointName, vectorName);
            IO.INI.WriteToFile(fileName, point);
            Points.Add(fileName);
        }

        private static string WriteVector(double[] vector)
        {
            var vct=vector.Aggregate(string.Empty, (seed, e) => seed + " " + e).Trim();
			return vct.Replace(".","_").Replace(",","_");
        }

        private static Task GenerateTask(string task, Settings settings)
        {
            var tree = new SimpleParser<Arithmetic, double>(double.Parse, t => true).Parse(task);
            var args = BulldozerSolutions.GenerateSample(tree.GetVariableCount(), 0, null, 
                                                             settings.SampleMinValue, 
                                                             settings.SampleMaxValue, 
                                                             settings.SampleDelta)
                                             .ToArray();
            var random = new Random();
            var control = args.Select(tree.ComplileDelegate<double>()).ToArray();
            var learn = control.Select(z => z * random.RandomDouble(1 - settings.SampleNoise, 1 + settings.SampleNoise))
                                              .ToArray();
            return new Task
                       {
                           DataSet = new TaskDataSet
                                         {
                                             Args = args, 
                                             Control = control, 
                                             Learn = learn
                                         },
                           Properties = new Properties
                                            {
                                                AlgorithmRunsCount = settings.AlgorithmRunsCount,
                                                IterationCount = settings.IterationCount,
                                                NoiseLevel = settings.SampleNoise,
                                                Threshold = settings.Threshold,
                                                TextRepresentation = task,
                                                OnlineSimplification = settings.OnlineSimplification,
                                                OnlineSimplificationRate = settings.OnlineSimplificationRate
                                            }
                       };
        }
    }
}