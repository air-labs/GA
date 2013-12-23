using AIRLab.Thornado;
using System;

namespace Balancer
{
    [Serializable]
    public class Task
    {
        [Thornado]
        public TaskDataSet DataSet { get; set; }
        [Thornado]
        public Properties Properties { get; set; }
        [Thornado]
        public string FileName { get; set; }
    }

    [Serializable]
    public class TaskDataSet
    {
        [Thornado]
        public double[][] Args { get; set; }
        [Thornado]
        public double[] Learn { get; set; }
        [Thornado]
        public double[] Control { get; set; }
    }

    [Serializable]
    public class Properties
    {
        [Thornado]
        public string TextRepresentation { get; set; }
        [Thornado]
        public int AlgorithmRunsCount { get; set; }
        [Thornado]
        public int IterationCount { get; set; }
        [Thornado]
        public double NoiseLevel { get; set; }
        [Thornado]
        public double Threshold { get; set; }
        [Thornado]
        public bool OnlineSimplification { get; set; }
        [Thornado]
        public int OnlineSimplificationRate { get; set; }
    }
}