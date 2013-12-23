using System;
using System.Collections.Generic;
using System.Linq;

namespace AIRLab.Bulldozer
{
    public class ConsoleGui : GeneticAlgorithms.ConsoleGui
    {
        private static List<TreeChromosome> chromosomes = new List<TreeChromosome>();
        public static void Run<T>(BulldozerAlgorithm<T> alg, int iterationShowRate, Func<int, bool> stop, String formula, int max_iteration)            
        {
            
            for (var cnt = 0; ; cnt++)
            {
                alg.MakeIteration();
                if (cnt % iterationShowRate != 0) continue;
                Console.Clear();
                var bound = Math.Min(alg.Pool.Count, 15);
                for (var i = 0; i < bound; i++)
                {
                    Console.SetCursorPosition(0, i);
                    Console.Write("{0}\t{1}", alg.Pool[i].Value, alg.Pool[i]);
                }
                Console.SetCursorPosition(0, 16);
                Console.WriteLine("Formula: " + formula);
                Console.WriteLine("Iterations:     " + alg.CurrentIteration);
                Console.WriteLine("Average value:  " + alg.Pool.Select(z => z.Value).Average());
                Console.WriteLine("Average age:    " + alg.Pool.Select(z => z.Age).Average());

                Console.WriteLine("Parameters:");

                //                if(!alg.IsParameterized) return;
                //                Console.SetCursorPosition(0, 20);
                // alg.Parameters.ForEach(p => Console.WriteLine("{0}\t{1}", p.Name, p.Value));
                if (stop(max_iteration)) break;
            }
        }
    }
}
