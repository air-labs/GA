using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AIRLab.CA;
using AIRLab.CA.Tools;
using AIRLab.GeneticAlgorithms;

namespace AIRLab.Bulldozer
{
    public class BulldozerAlgorithm<T> : GeneticAlgorithm<TreeChromosome>
    {
        public readonly List<RuleSet> MutationRules = new List<RuleSet>();
        public readonly List<RuleSet> CrossoverRules = new List<RuleSet>();
        public readonly List<RuleSet> OnlineRules = new List<RuleSet>();
        public readonly List<Metric> Metrics = new List<Metric>();
        
        private Roulette _mutationRoulette;
        private readonly LambdaCache<T> _cache;

        private const int MinPoolSize = 30;
        private const int MaxPoolSize = 30;

        private const int CacheSize = 1000;

        public BulldozerAlgorithm() 
            : base(null)
        {
            IterationBegins+=IterationBeings;
            Solutions.AppearenceCount.MinimalPoolSize(this, MinPoolSize);
            Solutions.MutationOrigins.Random(this, 0.5);
            Solutions.CrossFamilies.Random(this, c => 0.3 * c);
            Solutions.CrossFamilies.Proportional(this, c => 0.3 * c);
            Solutions.Selections.Threashold(this, MaxPoolSize);
            Evaluate = MakeEvaluate;
            PerformAppearence = Appearence;
            PerformMutation = Mutation;
            PerformCrossover = Cross;
            ReevaluateOldGenes = true;
            _cache = new LambdaCache<T>(CacheSize);
        }
        
        TreeChromosome Mutation(TreeChromosome g)
        {
            var parent = g.ID;
            var index = _mutationRoulette.RandomSector();
            var set = MutationRules[index];
            Rule rule;
            var result = set.ApplyRandomly(g.Tree, Rnd, out rule);
            if (result == null) return null;
            var chromosome = new TreeChromosome(result);
            chromosome.HistoryTag = string.Format("{0}\t{1}\t{2} {3}\t{4}", chromosome.ID, chromosome.Tree, set.Name, rule.Name, parent);
            return chromosome;
        }

        private TreeChromosome Cross(TreeChromosome g1, TreeChromosome g2)
        {
            var parent1 = g1.ID;
            var parent2 = g2.ID;
            if (g1 == g2) return null;
            var r = Rnd.RandomElement(CrossoverRules.SelectMany(set => set.Rules));
            if (r == null) return null;
            var instances = r.SelectWhere(g1.Tree,g2.Tree).ToList();
            if (!instances.Any()) return null;
            var ins = Rnd.RandomElement(instances);
            var result=r.Apply(ins);
            if (result == null) return null;
            var chromosome = new TreeChromosome(result[0]);
            chromosome.HistoryTag = string.Format("{0}\t{1}\tCrossing\t{2} {3}", chromosome.ID, chromosome.Tree, parent1, parent2);
            return chromosome;
        }

        private static TreeChromosome Appearence()
        {
            return new TreeChromosome(new Constant<T>(default(T)));
        }

        private void MakeEvaluate(TreeChromosome c)
        {
            c.Metrics = Metrics.Select(z => z.Func(c)).ToArray();
            c.Value = Metrics.Select((z, i) => c.Metrics[i]*z.Weight).Sum();
        }

        public void SimplifyPool(List<Rule> simplificationRules)
        {
            for (var i = 0; i < Pool.Count; ++i)
                Pool[i] = SimplifyChromosome(Pool[i], simplificationRules);
            foreach (var g in Pool)
                Evaluate(g);
        }

        public TreeChromosome SimplifyChromosome(TreeChromosome treeChromosome, List<Rule> simplificationRules)
        {
            return new TreeChromosome(Simplifier.Simplify(treeChromosome.Tree, simplificationRules));
        }

        private void IterationBeings(object sender, EventArgs ev)
        {
            var mutationWidthes = MutationRules.Select(set => set.Weight);
            _mutationRoulette = Rnd.CreateRoulette(mutationWidthes, 0);
        }

        public Func<IList, T> GetLambda(TreeChromosome treeChromosome)
        {
            return _cache.GetLambda(treeChromosome);
        }
    }
}
