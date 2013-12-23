using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AIRLab.Bulldozer;
using AIRLab.CA;
using AIRLab.GeneticAlgorithms;

namespace Balancer
{
    public static class DoubleApproximationAssembler
    {
        private const double EvalThreshold = 0.95;
        private const double EvalEquiv = 0.0001;
        private const int TolerateAge = 2;

        private static BulldozerAlgorithm<double> _alg;
        private static TaskDataSet _dataSet;

        public static BulldozerAlgorithm<double> AssembleAlgorithm(TaskDataSet dataSet, Dictionary<string, double> weights, Dictionary<string, double> merics, bool onlineSimplification)
        {
            _dataSet = dataSet;

            _alg = new BulldozerAlgorithm<double>();

            AlgebraicRules.RandomMin = -10;
            AlgebraicRules.RandomMax = 10;

            var mutationSet = new List<RuleSet>
                                  {
                                      new RuleSet
                                          {
                                              Name = StdTags.SafeResection,
                                              Weight = weights[StdTags.SafeResection],
                                              Rules = AlgebraicRules.Get()
                                                                    .Where(r => r.Tags.Contains(StdTags.SafeResection))
                                                                    .ToList()
                                          },
                                      new RuleSet
                                          {
                                              Name = StdTags.UnsafeResection,
                                              Weight = weights[StdTags.UnsafeResection],
                                              Rules = new List<Rule>(BasicRules.MutationRules())
                                          },
                                      new RuleSet
                                          {
                                              Name = StdTags.SafeBlowing,
                                              Weight = weights[StdTags.SafeBlowing],
                                              Rules = AlgebraicRules.Get()
                                                                    .Where(r => r.Tags.Contains(StdTags.SafeBlowing))
                                                                    .ToList()
                                          },
                                      new RuleSet
                                          {
                                              Name = StdTags.UnsafeBlowing,
                                              Weight = weights[StdTags.UnsafeBlowing],
                                              Rules = AlgebraicRules.Get()
                                                                    .Where(r => r.Tags.Contains(StdTags.UnsafeBlowing))
                                                                    .Union(BasicRules.VariableRules(_dataSet.Args.First().Length))
                                                                    .ToList()
                                          },
                                      new RuleSet
                                          {
                                              Name = StdTags.Tunning,
                                              Weight = weights[StdTags.Tunning],
                                              Rules = AlgebraicRules.Get()
                                                                    .Where(r => r.Tags.Contains(StdTags.Tunning))
                                                                    .ToList()
                                          }
                                  };

            var inductiveSet = mutationSet.Where(s => s.Name != StdTags.SafeResection
                                                   && s.Name != StdTags.UnsafeResection)
                                          .ToList();

            _alg.MutationRules.AddRange(!onlineSimplification ? mutationSet : inductiveSet);
            _alg.CrossoverRules.Add(new RuleSet
                                        {
                                            Name = "Crossing",
                                            Rules = BasicRules.CrossingRules().ToList()
                                        });
            _alg.Metrics.Add(new Metric(tc => GetReciprocalError(tc, _dataSet.Learn), "Eval", 1));
            _alg.Metrics.Add(new Metric(tc => -tc.Tree.GetOperationCount(), "Length", merics["Length"]));
            _alg.Metrics.Add(new Metric(tc => GetReciprocalError(tc, _dataSet.Control), "CheatEval", 0));
           
            if (Environment.OSVersion.Platform != PlatformID.Unix)
                _alg.IterationCallBack += Print;

            _alg.PerformBanking = () => _alg.Bank.AddRange(_alg.Pool.Where(z => z.Age > 20 && z.Metrics[0] < EvalThreshold));
            _alg.PerformSelection = GASolutions.PerformSelection(ExceptCondition, 30);
            return _alg;
        }
        
        static void Print()
        {
            if (_alg.CurrentIteration % 10 != 0) return;
            Console.Clear();
            var bound = Math.Min(_alg.Pool.Count, 15);
            for (var i = 0; i < bound; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("{0}\t{1}", _alg.Pool[i].Value, _alg.Pool[i]);
            }
            Console.SetCursorPosition(0, 16);
            
            Console.WriteLine("Iterations:     " + _alg.CurrentIteration);
            Console.WriteLine("Average value:  " + _alg.Pool.Select(z => z.Value).Average());
            Console.WriteLine("Average age:    " + _alg.Pool.Select(z => z.Age).Average());
            Console.WriteLine("Bank size:      " + _alg.Bank.Count);
        }

        private static double GetReciprocalError(TreeChromosome tc, double[] answers)
        {
            var lambda = _alg.GetLambda(tc);
            return BulldozerSolutions.GetNumericDistance(answers, _dataSet.Args.Select(e => lambda(e)));
        }

        private static bool ExceptCondition(TreeChromosome c)
        {
            return c.Age > TolerateAge
                   && c.Metrics[0] < EvalThreshold
                   && _alg.Bank.Count(x => Math.Abs(x.Metrics[0] - c.Metrics[0]) < EvalEquiv) != 0;
        }
    }
}