using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIRLab.Thornado
{
    public static class ForEachExtension
    {
        public static void ForEach<T>(this IEnumerable<T> en, Action<T> action)
        {
            foreach (var e in en)
                action(e);
        }
    }
}
