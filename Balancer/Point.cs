using System.Collections.Generic;
using AIRLab.Thornado;
using System;

namespace Balancer
{
    [Serializable]
    public class Point
    {
        [Thornado]
        public Dictionary<string, double> Tags { get; set; }
        [Thornado]
        public Dictionary<string, double> Metrics { get; set; }
        [Thornado]
        public string FileName { get; set; }
    }
}