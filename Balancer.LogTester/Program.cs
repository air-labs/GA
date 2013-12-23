using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using AIRLab.Thornado;
using TaskGenerator;
using AIRLab.CA;
using System.Threading;
using System.Globalization;
using AIRLab.Bulldozer;

namespace Balancer.LogTester
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");

            var dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog();
            if (result != DialogResult.OK) { return; }

            var _settings = IO.INI.ParseFile<Settings>(Path.Combine(dialog.SelectedPath, "Settings.ini"));

            var _files = Directory.GetFiles(dialog.SelectedPath)
                              .Where(name => name.EndsWith(".ini"))
                              .Where(name => name.Contains("T-") && name.Contains("P-"))
                              .Select(file => IO.INI.ParseFile<StatisticItem>(file))
                              .ToList();

            int total = 0, successOn20000 = 0;
            foreach (var i in _files)
            {
                foreach (var ii in i.RunInfo)
                {
                    ++total;
                    if (ii.Success && ii.ElapsedIterations == i.Task.Properties.IterationCount)
                    {
                        ++successOn20000;
                    }
                }
            }

            Console.WriteLine(string.Format("Total: {0}\nSuccess on 20000: {1}", total, successOn20000));

            foreach (var i in _files)
            {
                foreach (var ii in i.RunInfo)
                {
                    var tree = GetTree(ii.ResultRepresentation);
                    var func = tree.ComplileDelegate<double>();
                    var res = i.Task.DataSet.Args.Select(a => func(a));
                    var eval = BulldozerSolutions.GetNumericDistance(res, i.Task.DataSet.Control);
                    var success = eval >= i.Task.Properties.Threshold;
                    if (success != ii.Success)
                    {
                        Console.WriteLine("FAIL");
                    }
                }
            }
            Console.WriteLine("Press any key");
            Console.ReadKey();
        }

        private static INode GetTree(string str)
        {
            var parser = new SimpleParser<Arithmetic, double>(double.Parse,
                t => t.IsGenericType && t.IsSubclassOf(typeof(BinaryOp)));
            var tree = parser.Parse(str);
            return tree;
        }
    }
}
