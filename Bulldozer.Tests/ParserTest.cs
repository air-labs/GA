using AIRLab.CA;
using NUnit.Framework;

namespace Balancer
{
    [TestFixture]
    public class ParserTest
    {
        [Test]
        public void ParserWorks()
        {
            var parser = new SimpleParser<Arithmetic, double>(double.Parse, 
                t => t.IsGenericType && t.IsSubclassOf(typeof(BinaryOp)));
            var tree = parser.Parse("  Plus ( Constant(5), VariableNode(0|x) ) ");
            Assert.AreEqual(tree.ToString(), "(5 + x)");
            Assert.AreEqual(tree.ToPrefixForm(), "Plus(Constant(5),VariableNode(0|x))");

            tree = parser.Parse("  Plus ( Product(VariableNode(1|y),Constant(5)), VariableNode(0|x) ) ");
            Assert.AreEqual(tree.ToString(), "((y ∙ 5) + x)");
            Assert.AreEqual(tree.ToPrefixForm(), "Plus(Product(VariableNode(1|y),Constant(5)),VariableNode(0|x))");
        }

    }
}
