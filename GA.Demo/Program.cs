using AIRLab.GeneticAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.Demo
{

    class MyChrom : ArrayChromosome<bool>
    {
        public int Difference;
        public override string ToString()
        {
            return Difference.ToString();
        }
        public MyChrom(int L) : base (L) {}
    }

    class Program
    {
        static void Main(string[] args)
        {
            var rnd=new Random(1);
            var weights=Enumerable.Range(0,1000).Select(z=>rnd.Next(1000)).ToArray();
            var S=weights.Sum();


            var alg = new GeneticAlgorithm<MyChrom>(() => new MyChrom(weights.Length));

            Solutions.AppearenceCount.MinimalPoolSize(alg, 100);
            Solutions.CrossFamilies.Proportional(alg, z => z / 3);
            Solutions.MutationOrigins.Random(alg, 0.75);
            Solutions.Selections.Threashold(alg, 80);

            alg.PerformAppearence =  () =>
                {
                    var c = new MyChrom(weights.Length);
                    for (int i = 0; i < c.Code.Length; i++)
                        c.Code[i] = alg.Rnd.Next(10) > 5;
                    return c;
                };


            alg.PerformMutation = c =>
                {
                    var copy = c.Clone() as MyChrom;
                    var e = alg.Rnd.Next(weights.Length);
                    copy.Code[e] = !copy.Code[e];
                    return c;
                };

            alg.PerformCrossover = (a, b) =>
                {
                    var child = new MyChrom(weights.Length);
                    for (int i = 0; i < child.Code.Length; i++)
                        if (alg.Rnd.Next(2) > 0) child.Code[i] = a.Code[i];
                        else child.Code[i] = b.Code[i];
                    return child;
                };

            alg.Evaluate = c =>
                {
                    var halfSum = c.Code.Select((value, index) => value ? 0 : weights[index]).Sum();
                    var otherHalf = S - halfSum;
                    c.Difference=Math.Abs(halfSum-otherHalf);
                    c.Value=1.0 / (Math.Abs(halfSum - otherHalf) + 1);
                };


            for (int i = 0; i < 10000; i++)
            {
                alg.MakeIteration();
                Console.Write("{0}\t\t{1}         \r", alg.Pool[0].Age, alg.Pool[0].Difference);
            }

        }
    }
}
