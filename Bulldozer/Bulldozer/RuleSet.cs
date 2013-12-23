using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AIRLab.CA;
using AIRLab.GeneticAlgorithms;
using AIRLab.Thornado;

namespace AIRLab.Bulldozer
{
    public class RuleSet
    {
        [Thornado]
        public double Weight { get; set; }
        [Thornado]
        public string Name { get; set; }
        [Thornado]
        public List<Rule> Rules { get; set; }

        public INode ApplySequentially(INode root)
        {
            throw new NotImplementedException();
        }

        public INode ApplyRandomly(INode root, Random rnd, out Rule rule)
        {
            var order = GeneratePermutation(Rules, rnd);
            foreach (var idx in order)
            {
                var r = Rules[idx];
                var instances = r.SelectWhere(root).ToList();
                if (!instances.Any()){ continue; }
                var ins = rnd.RandomElement(instances);
                var roots = r.Apply(ins);
                if (roots != null)
                {
                    rule = r;
                    return roots[0];
                }
            }
            rule = null;
            return null;
        }

        private static IEnumerable<int> GeneratePermutation(ICollection rules, Random rnd)
        {
            var order = Enumerable.Repeat(int.MinValue, rules.Count)
                                  .ToList();
            for (var i = 0; i < order.Count; ++i)
            {
                var idx = rnd.Next(order.Count);
                var actualPlace = order[idx] < 0 
                    ? idx 
                    : order.Select((element, index) => Tuple.Create(element, index))
                           .First(e => e.Item1 == int.MinValue)
                           .Item2;
                order[actualPlace] = i;
            }
            return order;
        }
    }
}
