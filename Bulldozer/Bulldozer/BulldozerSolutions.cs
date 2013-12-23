using System;
using System.Collections.Generic;
using System.Linq;

namespace AIRLab.Bulldozer
{
    public static class BulldozerSolutions
    {
        public static double GetNumericDistance(IEnumerable<double> first, IEnumerable<double> second)
        {
            return GetDistance(first, second, (f, s) => Math.Abs(f - s));
        }

        private static double GetDistance<T>(IEnumerable<T> first, IEnumerable<T> second, Func<T,T,double> zipper)
        {
            var zip = first.Zip(second, zipper).ToList();
            var result = zip.Sum() / zip.Count;
            if (Double.IsNaN(result) || Double.IsInfinity(result)) return -1;
            return 1 / (result + 1);
        }

        public static double GetBooleanDistance(IEnumerable<bool> first, IEnumerable<bool> second)
        {
            return GetDistance(first, second, (f, s) => f != s ? 1 : 0);
        }

        public static IEnumerable<double[]> GenerateSample(int count, int step, double[] temp, double min, double max, double delta)
        {
            if (count == 0)
                yield break;

            if (temp == null)
                temp = new double[count];

            for (var v = min; v <= max; v += delta)
            {
                temp[step] = v;
                if (step != count - 1)
                    foreach (var e in GenerateSample(count, step + 1, temp, min, max, delta))
                        yield return e;
                else
                    yield return temp.ToArray();
            }
        }
    }
}