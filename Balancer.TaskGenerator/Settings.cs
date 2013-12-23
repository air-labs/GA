using System;
using AIRLab.Thornado;

namespace TaskGenerator
{
    public class Settings
    {
        [Thornado]
        public string[] Tasks { get; set; }
        
        [Thornado]
        public int SampleDelta { get; set; }

        [Thornado]
        public double SampleMinValue { get; set; }

        [Thornado]
        public double SampleMaxValue { get; set; }

        [Thornado]
        public double SampleNoise { get; set; }

        [Thornado]
        public int AlgorithmRunsCount { get; set; }

        [Thornado]
        public int IterationCount { get; set; }

        [Thornado]
        public double Threshold { get; set; }

        [Thornado]
        public Parameter[] Metrics { get; set; }

        [Thornado]
        public Parameter[] SetWeights { get; set; }

        [Thornado]
        public bool OnlineSimplification { get; set; }

        [Thornado]
        public int OnlineSimplificationRate { get; set; }
    }

    [Serializable]
    public class Parameter
    {
        [Thornado]
        public string[] Names { get; set; }

        [Thornado]
        public double MinValue { get; set; }

        [Thornado]
        public double MaxValue { get; set; }

        [Thornado]
        public int PointCount { get; set; }

        [Thornado]
        public GenerationType GenerationType { get; set; }

        [Thornado]
        public double DefaulValue { get; set; }

        [Thornado]
        public double[] Sequence { get; set; }
    }

    public enum GenerationType
    {
        Linear,
        Exponental,
        Constant,
        Sequence
    }
}