using System.Collections.Generic;
using AIRLab.Bulldozer;
using AIRLab.Thornado;
using System;

namespace Balancer
{
    [Serializable]
    public class StatisticItem
    {
        public StatisticItem()
        {
            RunInfo = new List<RunInfo>();
            Rules = new List<RuleSet>[]{};
        }

        [Thornado]
        public Task Task { get; set; }
        [Thornado]
        public Point Point { get; set; }
        [Thornado]
        public List<RuleSet>[] Rules { get; set; }
        [Thornado]
        public List<RunInfo> RunInfo { get; set; }
        [Thornado]
        public double SuccessRate { get; set; }
        [Thornado]
        public double TotalElapsedSeconds { get; set; }
    }

    [Serializable]
    public class RunInfo
    {
        [Thornado]
        public bool Success { get; set; }
        [Thornado]
        public string ResultRepresentation { get; set; }
        [Thornado]
        public double ElapsedSeconds { get; set; }
        [Thornado]
        public int ElapsedIterations { get; set; }
        [Thornado]
        public long ID { get; set; }

        public List<GeneHistory> History { get; set; }
    }

    [Serializable]
    public class GeneHistory 
    {
        public int AppearIterations { get; set; }
        public int Age { get; set; }
        public Dictionary<string, double> Marks { get; set; }
        public Dictionary<string, int> Tags { get; set; }
    }
}