using System;

namespace AIRLab.Bulldozer
{
    public class Metric
    {
        public Func<TreeChromosome, double> Func { get; set; }
        public double Weight { get; set; }
        public string Name { get; set; }
        
        public Metric(Func<TreeChromosome, double> metricFunction, string name, double weight = 0.5)
        {
            Weight = weight;
            Name = name;
            Func = metricFunction;
        }

        public double Calculate(TreeChromosome c)
        {
            return Func(c) * Weight;
        }
    }
}
