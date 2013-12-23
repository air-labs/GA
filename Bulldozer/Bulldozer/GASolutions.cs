using System;
using System.Collections.Generic;
using System.Linq;

namespace AIRLab.Bulldozer
{
    public static class GASolutions
    {
        public static Action<List<TreeChromosome>, List<TreeChromosome>> PerformSelection(
            Func<TreeChromosome, bool> excludeCondition, int take)
        {
            return (from, to) =>
                       {
                           var exclude = from.Where(excludeCondition);
                           var e1 = from.Except(exclude);
                           var e2 = e1.OrderBy(z => -z.Value).Take(take);
                           to.AddRange(e2);
                       };
        }
    }
}