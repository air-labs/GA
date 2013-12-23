using System;
using System.Linq;
using AIRLab.Thornado;

namespace Balancer
{
    public static class Balancer
    {
        private static string _taskName;
        private static string _pointName;

        public static bool ShowTrace = false;
        public static bool TraceEvolution = true;

        public static void Main(string[] args)
        {
            if(args.Length != 2)
            {
                Console.WriteLine("Usage: Balancer.exe task.ini point.ini");
                return;
            }

            _taskName = args[0];
            _pointName = args[1];

            Console.WriteLine("Reading arguments");
            var runner = new DoubleBulldozerBalancer(ReadTask(), ReadPoint());
            WriteStatisticItem(runner.Run());
        }

        private static void WriteStatisticItem(StatisticItem item)
        {
            Console.WriteLine("Writing result");
            var fileName = string.Format("T-{0}_P-{1}.ini", _taskName.Split('.').First(), _pointName.Split('.').First());
            IO.INI.WriteToFile(fileName, item);
            Console.WriteLine("Result written");
        }

        private static Point ReadPoint()
        {
            var point = IO.INI.ParseFile<Point>(_pointName);
            point.FileName = _pointName;
            Console.WriteLine("Point parsed");
            return point;
        }

        private static Task ReadTask()
        {
            var task = IO.INI.ParseFile<Task>(_taskName);
            task.FileName = _taskName;
            Console.WriteLine("Task parsed");
            return task;
        }
    }
}
