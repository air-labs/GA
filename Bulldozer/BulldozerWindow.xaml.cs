using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using GeneticAlgorithms;
using WpfMath;

namespace Bulldozer
{
    public partial class BulldozerWindow
    {
        private readonly GeneticAlgorithm<TreeChromosome> _alg;
        private readonly Metric[] _metrics;
        private readonly int _iterationShowRate;
        private readonly GaDispatcher _dispatcher;
        private bool _manualStop;
        private Grid _poolView;

        public BulldozerWindow(GeneticAlgorithm<TreeChromosome> alg, Metric[] metrics, int iterationShowRate)
        {
            InitializeComponent();
            TexFormulaParser.Initialize();
            _alg = alg;
            _metrics = metrics;
            _iterationShowRate = iterationShowRate;
            _dispatcher = new GaDispatcher(alg);
            _manualStop = false;
            _alg.IterationCallBack += AlgIterationCallBack;
            FillPoolPanel();
            FillOptionalPanel();
        }

        private void FillPoolPanel()
        {
            _poolView = new Grid();
            _poolView.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            _poolView.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            _poolView.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            _poolView.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            var number = new Label { Content = "Number" };
            _poolView.Children.Add(number);
            Grid.SetColumn(number, 0); Grid.SetRow(number, 0);
            var mark = new Label { Content = "Mark" };
            _poolView.Children.Add(mark);
            Grid.SetColumn(mark, 1); Grid.SetRow(mark, 0);
            var formula = new Label { Content = "Formula" };
            _poolView.Children.Add(formula);
            Grid.SetColumn(formula, 2); Grid.SetRow(formula, 0);
            for (var i = 1; i < 6; i++)
            {
                _poolView.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                var currentNumber = new Label {Content = i+":"};
                _poolView.Children.Add(currentNumber);
                Grid.SetColumn(currentNumber, 0); Grid.SetRow(currentNumber, i);
                var currentMark = new Label();
                _poolView.Children.Add(currentMark);
                Grid.SetColumn(currentMark, 1); Grid.SetRow(currentMark, i);
                var currentFormula = new Label {Content = new VisualContainerElement()};
                _poolView.Children.Add(currentFormula);
                Grid.SetColumn(currentFormula, 2); Grid.SetRow(currentFormula, i);
            }
            PoolPanel.Content = _poolView;
        }

        private void FillOptionalPanel()
        {
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            _metrics.ForEach(delegate { grid.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto}); });
            for (var i = 0; i < _metrics.Length; i++)
            {
                var name = new Label {Content = _metrics[i].Name};
                grid.Children.Add(name);
                Grid.SetColumn(name, 0); Grid.SetRow(name, i);
                var slider = new Slider
                                 {
                                     Value = _metrics[i].Weight,
                                     Minimum = 0,
                                     Maximum = 1,
                                     TickFrequency = .1,
                                     Width = 200,
                                     IsSnapToTickEnabled = true
                                 };
                grid.Children.Add(slider);
                Grid.SetColumn(slider, 1); Grid.SetRow(slider, i);
                var label = new Label { Content = _metrics[i].Weight };
                grid.Children.Add(label);
                Grid.SetColumn(label, 2); Grid.SetRow(label, i);

                var idx = i;
                slider.ValueChanged += (sender, e) =>
                                           {
                                               _metrics[idx].Weight = e.NewValue.Round(1);
                                               label.Content = e.NewValue.Round(1);
                                           };
            }
            OptionalPanel.Content = grid;
        }

        private void AlgIterationCallBack()
        {
            if (_manualStop) return;
            if (_alg.Pool.Count < 5) return;
            if (_alg.CurrentIteration%_iterationShowRate == 0)
                Dispatcher.Invoke(DispatcherPriority.Background,
                                  FullUpdateAction(_alg.Pool.Select(g => g.Clone<TreeChromosome>()).ToArray(),
                                            _alg.CurrentIteration));
        }

        private Action FullUpdateAction(TreeChromosome[] pool, int currentIteration)
        {
            return () =>
                       {
                           PoolUpdateAction(pool);
                           Iterations.Content = currentIteration;
                           AverageValue.Content = pool.Select(z => z.Value).Average();
                           AverageAge.Content = (int) pool.Select(z => z.Age).Average();
                       };
        }

        private void PoolUpdateAction(TreeChromosome[] pool)
        {
            for (var i = 0; i < 5; i++)
            {
                var idx = i;
                var markCell = (Label)_poolView.Children
                    .Cast<UIElement>()
                    .Single(e => Grid.GetRow(e) == idx + 1 && Grid.GetColumn(e) == 1);
                markCell.Content = pool[i].Value;
                var formulaCell = (Label)_poolView.Children
                    .Cast<UIElement>()
                    .Single(e => Grid.GetRow(e) == idx + 1 && Grid.GetColumn(e) == 2);
                ((VisualContainerElement) formulaCell.Content).Visual = GetVisual(pool[i].ToString());
            }
        }

        private static DrawingVisual GetVisual(string formula)
        {
            var visual = new DrawingVisual();
            using (var drawingContext = visual.RenderOpen())
            {
                new TexFormulaParser().Parse(formula)
                    .GetRenderer(TexStyle.Display, 15d)
                    .Render(drawingContext, 0, 1);
            }
            return visual;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            _dispatcher.StartIterations();
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            _dispatcher.StopIterations();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PauseClick(object sender, RoutedEventArgs e)
        {
            _manualStop = true;
            _dispatcher.PauseIterations();
        }

        private void ResumeClick(object sender, RoutedEventArgs e)
        {
            _manualStop = false;
            _dispatcher.ResumeIterations();
        }

        private void RefreshClick(object sender, RoutedEventArgs e)
        {
            _manualStop = true;
            _dispatcher.RefreshIterations();
            _manualStop = false;
        }
    }
}
