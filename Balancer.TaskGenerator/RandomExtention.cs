using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskGenerator
{
    public static class RandomExtensions
    {
        public static double RandomDouble(this Random rnd, double min, double max)
        {
            return min + (max - min) * (rnd.NextDouble());
        }
    }
}
