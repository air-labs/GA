using System;
using AIRLab.CA;
using AIRLab.GeneticAlgorithms;

namespace AIRLab.Bulldozer
{
    public class TreeChromosome : Chromosome
    {
        public TreeChromosome(INode tree)
        {
            tree.MakeRoot();
            Tree = tree;
            ID = Identity.NewIdentity();
            HistoryTag = string.Format("{0}\t{1}\t_\t_", ID, Tree);
        }

        public INode Tree { get; private set; }
        public long ID { get; private set; }
        public string HistoryTag { get; set; }

        public override bool Equals(Chromosome other)
        {
            return Tree.ToString() == ((TreeChromosome)other).Tree.ToString();
        }

        public override string ToString()
        {
            return Tree.ToString();
        }

        public override object Clone()
        {
            var clone = MemberwiseClone();
            var tree = (TreeChromosome) clone;
            tree.Tree = Tree.Clone<INode>();
            tree.ID = ID;
            return clone;
        }

        public double[] Metrics { get; set; }
    }
}