using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace EvolutionViewer
{
    public partial class Form1 : Form
    {
        public Form1(List<Tuple<int, int>> pairs)
        {
            var chart = new Chart();

            var area = new ChartArea();
            chart.ChartAreas.Add(area);

            var series = new Series();
            series.ChartType = SeriesChartType.Point;
            
            foreach(var p in pairs)
            {
                series.Points.AddXY(p.Item1, p.Item2);
            }
            chart.Series.Add(series);

            this.Controls.Add(chart);
            InitializeComponent();
        }
    }
}
