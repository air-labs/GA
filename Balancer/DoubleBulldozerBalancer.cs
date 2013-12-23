using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AIRLab.Bulldozer;
using AIRLab.CA;
using AIRLab.GeneticAlgorithms;

namespace Balancer
{
    public class DoubleBulldozerBalancer
    {
        private readonly Task _task;
        private readonly Point _point;
        private readonly StatisticItem _item;
        private const string _evolutionFormat = @"Evolution_T-{0}_P-{1}_R-{2}.txt";

        public DoubleBulldozerBalancer(Task task, Point point)
        {
            _task = task;
            _point = point;
            _item = new StatisticItem
                        {
                            Task = _task, 
                            Point = _point
                        };
        }

        public StatisticItem Run()
        {
            var algorithm = DoubleApproximationAssembler
                .AssembleAlgorithm(_task.DataSet, _point.Tags, _point.Metrics, _task.Properties.OnlineSimplification);
            _item.Rules = new[] {algorithm.MutationRules, algorithm.CrossoverRules};
            for(var i = 0; i < _task.Properties.AlgorithmRunsCount; ++i)
            {
                Identity.Reset();
                var evolutionLogName = string.Format(_evolutionFormat, _task.FileName.Split('.').First(), _point.FileName.Split('.').First(), i + 1);
                _item.RunInfo.Add(RunOnce(algorithm, _task.Properties, evolutionLogName));
                algorithm.Restart();
            }
            _item.SuccessRate = (double)_item.RunInfo.Count(i => i.Success)/_item.RunInfo.Count;
            _item.TotalElapsedSeconds = _item.RunInfo.Sum(i => i.ElapsedSeconds);
            return _item;
        }

        private static RunInfo RunOnce(BulldozerAlgorithm<double> algorithm, Properties properties, string evolutionLogName)
        {
            var success = false;
            Chromosome chromosome = null;
            var simplificationRules = AlgebraicRules.Get()
                                                    .Where(z => z.Tags.Contains(StdTags.SafeResection))
                                                    .ToList();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (var i = 0; i < properties.IterationCount; ++i)
            {
                algorithm.MakeIteration();

                if (OnlineSimplification(algorithm, properties))
                {
                    if (Balancer.TraceEvolution) WriteEvolutionLog(algorithm, evolutionLogName);
                    algorithm.SimplifyPool(simplificationRules);
                }

                if (Balancer.ShowTrace) Trace(algorithm);
                if (Balancer.TraceEvolution) WriteEvolutionLog(algorithm, evolutionLogName);

                success = IsSuccess(algorithm, properties.Threshold, out chromosome);
                if(success) 
                { 
                    break; 
                }
            }
            stopwatch.Stop();

            var treeChromosome = (TreeChromosome)chromosome;
            return new RunInfo
                       {
                           Success = success,
                           ElapsedSeconds = stopwatch.Elapsed.TotalSeconds,
                           ElapsedIterations = algorithm.CurrentIteration,
                           ID = chromosome != null ? treeChromosome.ID : -1,
                           ResultRepresentation = chromosome != null
                                                      ? treeChromosome.Tree.ToPrefixForm()
                                                      : null
                       };
        }

        private static void WriteEvolutionLog(BulldozerAlgorithm<double> algorithm, string logName)
        {
            using (var file = File.Open(logName, FileMode.Append))
            {
                var text = new StreamWriter(file) {AutoFlush = true};
                text.WriteLine("[{0}]", algorithm.CurrentIteration);
                var tags = algorithm.Pool.Select(GetTag);
                tags.ForEach(text.WriteLine);
            }
        }

        private static string GetTag(TreeChromosome c) 
        {
            var historyTag = c.HistoryTag;
            var metrics = c.Metrics.Aggregate(string.Empty, (s, m) => s + " " + m);
            return string.Format("{0}\t{1}", historyTag, metrics);
        }

        private static void Trace(BulldozerAlgorithm<double> algorithm)
        {
            if(algorithm.CurrentIteration % 100 != 0) return;
            Console.Clear();
            Console.SetCursorPosition(0,0);
            Console.WriteLine("===============================");
            algorithm.ChromosomePool.Union(algorithm.ChromosomeBank)
                .OfType<TreeChromosome>()
                .OrderByDescending(c => c.Value)
                .Take(5)
                .Select(c => Tuple.Create(c.Tree.ToPrefixForm(), c.Value))
                .ForEach(p => Console.WriteLine("{0}   {1}", p.Item1, p.Item2));
            Console.WriteLine("===============================");
            Console.WriteLine();
        }

        private static bool OnlineSimplification(BulldozerAlgorithm<double> algorithm, Properties properties)
        {
            return properties.OnlineSimplification 
                && algorithm.CurrentIteration % properties.OnlineSimplificationRate == 0;
        }

        private static bool IsSuccess(BulldozerAlgorithm<double> algorithm, double threshold, out Chromosome chromosome)
        {
            var chromosomes = algorithm.ChromosomePool
                                       .Union(algorithm.ChromosomeBank)
                                       .OfType<TreeChromosome>()
                                       .ToList();
            var maxValue = chromosomes.Max(c => c.Metrics[2]);
            chromosome = chromosomes.FirstOrDefault(c => c.Metrics[2] == maxValue);
            return maxValue >= threshold;
        }
    }
}