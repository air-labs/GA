using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using AIRLab.Thornado;
using Balancer;
using System.Runtime.Serialization.Formatters.Binary;

namespace BalancerDbAggregator
{
    public class Program
    {
        private static readonly Regex pattern = new Regex(@"T-.+_P-.+\.ini");
        private const string _evolutionFormat = @"Evolution_T-{0}_P-{1}_R-{2}.txt";

        public static void Main(string[] args)
        {
            var path = args.FirstOrDefault();
            if (path == null) 
            { path = string.Empty; }
            var fileNames = Directory.GetFiles(path);
            var itemNames = fileNames.Where(file => pattern.IsMatch(file)).ToArray();
            var items = itemNames.Select(itemName => IO.INI.ParseFile<StatisticItem>(itemName)).ToArray();

            using (var stream = File.OpenWrite("balancer.db"))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, items);
                stream.Flush();
            }
        }

        private static GeneHistory GetHistory(string logName, string id)
        {
            var log = ReadLog(logName);
            var lines = log.SelectMany(group => group);
            var path = GetNodes(id, lines).Reverse();
            return new GeneHistory
            {
                Age = 0,
                AppearIterations = path.Count(),
                Tags = path.Select(line => line[2])
                           .GroupBy(line => line)
                           .ToDictionary(group => group.Key, group => group.Count()),
                Marks = new Dictionary<string, double>()
            };
        }

        private static List<List<string[]>> ReadLog(string logName)
        {
            var groups = new List<List<string[]>>();
            var strings = File.ReadAllLines(logName);
            var n = 0;

            foreach (var l in strings)
            {
                if (l.StartsWith("[") && l.EndsWith("]"))
                {
                    n = int.Parse(l.Trim('[', ']')) - 1;
                    groups.Add(new List<string[]>());
                    continue;
                }
                groups[n].Add(l.Split('\t'));
            }
            return groups;
        }

        private static IEnumerable<string[]> GetNodes(string id, IEnumerable<string[]> lines)
        {
            var line = lines.LastOrDefault(l => l[0] == id);
            if (line == null)
            {
                return new List<string[]>();
            }
            var newId = line[3];
            return (new[] { line }).Union(GetNodes(newId, lines));
        }
    }
}
