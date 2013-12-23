using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace EvolutionViewer
{
    // 0 - id
    // 4 - parent id
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var groups = new List<List<string[]>>();
            var strings = File.ReadAllLines("Test.txt");
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

            var lines = groups.SelectMany(g => g);

            var ids = lines.Select(l => l[0])
                           .Distinct();

            var paths = ids.Select(id => GetNodes(id, lines).Reverse()).ToList();

            var pairs = paths.Select(p => Tuple.Create(p.Count(), GetPartCount(p, lines)))
                             .ToList();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(pairs));
        }

        private static int GetPartCount(IEnumerable<string> path, IEnumerable<string[]> lines) 
        {
            return path.Where(e => e != "_")
                       .Select(id => lines.Last(l => l[0] == id))
                       .Where(l => l[2].Contains("UnsafeResection"))
                       .Count();
        }

        private static IEnumerable<string> GetNodes(string id, IEnumerable<string[]> lines) 
        {
            var line = lines.LastOrDefault(l => l[0] == id);
            if (line == null) 
            {
                return new[] { "_" };
            }
            var newId = line[3];
            return (new[] { id }).Union(GetNodes(newId, lines));
        }
    }
}
