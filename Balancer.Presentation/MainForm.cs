using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using AIRLab.Thornado;
using Balancer;
using TaskGenerator;
using System.Linq;
using AIRLab.CA;
using System.Threading;
using System.Globalization;

namespace Presenter
{
    public partial class MainForm : Form
    {
        private List<Item> _items;
        private Settings _settings;
        private Dictionary<string, bool> _activeFormulas;
        private List<Rect> _rectangles;
        private List<StatisticItem> _files;
        private string _x = "Length";
        private string _y = StdTags.UnsafeResection;

        public MainForm()
        {
            InitializeComponent();
            plot.Paint += (_, __) => UpdateView();
            plot.MouseClick += PlotOnMouseClick;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");
            comboBox1.SelectedIndex = 0;
            comboBox1.SelectedIndexChanged += delegate { plot.Refresh(); };
        }

        private void PlotOnMouseClick(object sender, MouseEventArgs mouseEventArgs)
        {
            var x = mouseEventArgs.X;
            var y = mouseEventArgs.Y;
            if(_rectangles == null) return;
            var rect = _rectangles.SingleOrDefault(r => x >= r.X && x <= r.X + r.Width
                                                     && y >= r.Y && y <= r.Y + r.Height);
            if (rect == null) return;

            dataBox.Text = rect.Data;
        }

        private void FillChartValues()
        {
            var first = _settings.Metrics.First(m => m.Names.First() == _x);
            var last = _settings.SetWeights.First(m => m.Names.First() == _y);

            ViewSettings.XCount = first.PointCount;
            ViewSettings.YCount = last.PointCount;
            ViewSettings.TotalWidth = plot.Width;
            ViewSettings.TotalHeight = plot.Height;
            ViewSettings.MaxX = first.MaxValue;
            ViewSettings.MinX = first.MinValue;
            ViewSettings.MaxY = last.MaxValue;
            ViewSettings.MinY = last.MinValue;
        }

        private static INode GetTree(string str) 
        {
            var parser = new SimpleParser<Arithmetic, double>(double.Parse,
                t => t.IsGenericType && t.IsSubclassOf(typeof(BinaryOp) ));
            var tree = parser.Parse(str);
            return tree;
        }

        private static double[] GenerateRow(Parameter parameter)
        {
            if (parameter.GenerationType == GenerationType.Constant)
            {
                return new[] { parameter.DefaulValue };
            }
            return Enumerable.Range(0, parameter.PointCount)
                             .Select(n => (double)n / (parameter.PointCount - 1))
                             .Select(n => Transfer(n, parameter))
                             .ToArray();
        }

        private static double Transfer(double num, Parameter parameter)
        {
            var max = parameter.MaxValue;
            var min = parameter.MinValue;
            return min + num * (max - min);
        }

        private void UpdateView()
        {
            if (_activeFormulas == null) { return; }
            var formulas = _activeFormulas.Where(p => p.Value)
                                          .Select(p => Regex.Match(p.Key, @"\d+.\s+(.+)").Groups[1].Value)
                                          .ToArray();
            if(formulas.Length == 0){ return; }
            var @where = _items.Where(item => formulas.Contains(item.Formula)).ToList();
            var @by = @where.GroupBy(item => new {
                                                     X = item.X.Value, 
                                                     Y = item.Y.Value
                                                 });
            _rectangles = @by.Select(group =>
                                            {
                                                var key = group.Key;
                                                return new Rect
                                                           {
                                                               X = ViewSettings.XIsZeroSegment ? 0 : ViewSettings.GetViewXPoint(key.X),
                                                               Y = ViewSettings.YIsZeroSegment ? 0 : ViewSettings.GetViewYPoint(key.Y),
                                                               Width = ViewSettings.CubeWidth, 
                                                               Height = ViewSettings.CubeHeight, 
                                                               Rate = group.Sum(item => item.SuccessRate),
                                                               Iterations = group.Sum(item => item.Iterations),
                                                               Length = group.Sum(item => item.Operations),
                                                               Data = group.Aggregate(string.Empty, (s, item) => GetTableByItem(item, s))
                                                           };
                                            })
                                .ToList();
            if (_rectangles.Count == 0) { return; }

            var xParameter = _settings.Metrics.First(m => m.Names.First() == _x);
            Linearize(xParameter, Axis.X);
            var yParameter = _settings.SetWeights.First(m => m.Names.First() == _y);
            Linearize(yParameter, Axis.Y);

            if (isCompleteView.Checked)
            {
                CompleteView(xParameter, yParameter);
            }

            using (var graphics = plot.CreateGraphics())
            {
                graphics.Clear(Color.White);
                if (comboBox1.SelectedIndex == 0)
                {
                    var max = _rectangles.Max(p => p.Rate);
                    var min = _rectangles.Min(p => p.Rate);

                    _rectangles.ForEach(point =>
                    {
                        var rate = (point.Rate - min) / (max - min);
                        var c = (byte)(rate * 255);
                        var brush = new SolidBrush(Color.FromArgb(255, 255, 255 - c, 255 - c));
                        var x = (float)point.X;
                        var y = (float)point.Y;
                        var sizeX = (float)ViewSettings.CubeWidth;
                        var sizeY = (float)ViewSettings.CubeHeight;
                        graphics.FillRectangle(brush, x, y, sizeX, sizeY);
                    });
                }
                if (comboBox1.SelectedIndex == 1)
                {
                    var max = _rectangles.Max(p => p.Iterations);
                    var min = _rectangles.Min(p => p.Iterations);

                    _rectangles.ForEach(point =>
                    {
                        var rate = (point.Iterations - min) / (float)(max - min);
                        var c = (byte)(rate * 255);
                        var brush = new SolidBrush(Color.FromArgb(255, c, 255, c));
                        var x = (float)point.X;
                        var y = (float)point.Y;
                        var sizeX = (float)ViewSettings.CubeWidth;
                        var sizeY = (float)ViewSettings.CubeHeight;
                        graphics.FillRectangle(brush, x, y, sizeX, sizeY);
                    });
                }
                if(comboBox1.SelectedIndex == 2)
                {
                    var max = _rectangles.Max(p => p.Length);
                    var min = _rectangles.Min(p => p.Length);

                    _rectangles.ForEach(point =>
                    {
                        var rate = (point.Length - min) / (float)(max - min);
                        var c = (byte)(rate * 255);
                        var brush = new SolidBrush(Color.FromArgb(255, c, c, 255));
                        var x = (float)point.X;
                        var y = (float)point.Y;
                        var sizeX = (float)ViewSettings.CubeWidth;
                        var sizeY = (float)ViewSettings.CubeHeight;
                        graphics.FillRectangle(brush, x, y, sizeX, sizeY);
                    });
                }
            }
        }

        private void CompleteView(Parameter xParameter, Parameter yParameter)
        {
            var crossJoint = from x in GenerateRow(xParameter)
                             from y in GenerateRow(yParameter)
                             select new {
                                 X = ViewSettings.GetViewXPoint(x),
                                 Y = ViewSettings.GetViewYPoint(y)
                             };
            crossJoint.ForEach(point =>
                                   {
                                       if(_rectangles.FirstOrDefault(rect => rect.X == point.X
                                                                          && rect.Y == point.Y) == null)
                                       {
                                           _rectangles.Add(new Rect
                                                               {
                                                                   X = point.X, 
                                                                   Y = point.Y, 
                                                                   Width = ViewSettings.CubeWidth,
                                                                   Height = ViewSettings.CubeHeight, 
                                                                   Data = "Missed",
                                                                   Rate = 0,
                                                                   Iterations = _settings.IterationCount
                                                               });
                                       }
                                   });
        }

        private void Linearize(Parameter parameter, Axis axis)
        {
            if (parameter.GenerationType != GenerationType.Exponental &&
                parameter.GenerationType != GenerationType.Sequence) return;
            var row = GenerateRow(parameter);
            var rects = _rectangles.GroupBy(r => axis == Axis.X ? r.X : r.Y)
                .Select(g => new {Key = g.Key, Rs = g.ToList()})
                .OrderBy(g => g.Key)
                .ToList();
            for (var i = 0; i < rects.Count; ++i)
            {
                foreach (var ee in rects[i].Rs)
                {
                    if (axis == Axis.X)
                    {
                        ee.X = ViewSettings.GetViewXPoint(row[i]);
                    }
                    else
                    {
                        ee.Y = ViewSettings.GetViewYPoint(row[i]);
                    }
                }
            }
        }

        private string GetTableByItem(Item item, string str)
        {
            var header = "F: " + item.Formula + ", S.R.: " + item.SuccessRate + ", It: " + item.Iterations + "\r\n" 
                       + "X: " + item.X.Value + " Y: " + item.Y.Value + "\r\n";
            var statItem = _files.FirstOrDefault(s => s.Point.Metrics[_x] == item.X.Value && s.Point.Tags[_y] == item.Y.Value && GetFormula(s.Task.Properties.TextRepresentation) == item.Formula);
            var table = statItem.RunInfo.Aggregate("\r\n", (s, i) => s + i.Success + "\t" + i.ElapsedIterations + "\t" + i.ElapsedSeconds + "\t" + GetFormula(i.ResultRepresentation) + "\r\n");
            return str + header + table + "\r\n";
        }

        private static string GetFormula(string str)
        {
            return GetTree(str).ToString();
        }

        private void AddCheckBox(string formula)
        {
            var checkBox = new CheckBox{Text = formula, AutoSize = true};
            checkBox.CheckedChanged += delegate
                                           {
                                               _activeFormulas[formula] ^= true;
                                               plot.Refresh();
                                           };
            stack.Controls.Add(checkBox);
        }

        private void CloseButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        private List<Item> GetData(string path)
        {
            _files = Directory.GetFiles(path)
                              .Where(name => name.EndsWith(".ini"))
                              .Where(name => name.Contains("T-") && name.Contains("P-"))
                              .Select(file => IO.INI.ParseFile<StatisticItem>(file))
                              .ToList();

            var items = _files.Select(item => new Item
                                                  {
                                                      Formula = GetFormula(item.Task.Properties.TextRepresentation),
                                                      SuccessRate = item.SuccessRate,
                                                      Y = item.Point.Tags.SingleOrDefault(m => m.Key == _y),
                                                      X = item.Point.Metrics.SingleOrDefault(m => m.Key == _x),
                                                      Iterations = item.RunInfo.Sum(info => info.ElapsedIterations)/item.RunInfo.Count,
                                                      Operations = item.RunInfo.Sum(info => GetTree(info.ResultRepresentation).GetOperationCount())/item.RunInfo.Count
                                                  });
            return items.ToList();
        }

        private class Item
        {
            public string Formula { get; set; }
            public double SuccessRate { get; set; }
            public int Iterations { get; set; }
            public int Operations { get; set; }
            public KeyValuePair<string, double> X { get; set; }
            public KeyValuePair<string, double> Y { get; set; }
        }

                private void OpenButtonClick(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog();
            if (result != DialogResult.OK) { return; }

            Clear();
            _settings = IO.INI.ParseFile<Settings>(Path.Combine(dialog.SelectedPath, "Settings.ini"));
            _items = GetData(dialog.SelectedPath);
            Fill();
        }

        private void Fill()
        {
            if (_items.Count == 0) { return; }

            FillChartValues();

            _activeFormulas = _items.GroupBy(it => it.Formula)
                                    .Select((g, i) => Tuple.Create(i + 1, g.Key))
                                    .ToDictionary(k => string.Format("{0}. {1}", k.Item1, k.Item2), k => false);
            foreach(var p in _activeFormulas)
            {
                AddCheckBox(p.Key);
            }
        }

        private void Clear()
        {
            stack.Controls.Clear();
            _items = null;
            _activeFormulas = null;
        }

        private void IsCompleteViewCheckedChanged(object sender, EventArgs e)
        {
            UpdateView();
        }
    }
}
