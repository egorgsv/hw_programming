using Matrix;
using Matrix.AlgebraicStructures;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    internal class SemigroupTest
    {
        private BooleanSemigroup sg;
        private Boolean t, f;

        public SemigroupTest()
        {
            sg = new BooleanSemigroup();
            f = new Boolean(false);
            t = new Boolean(true);
        }

        [Test]
        public void TestMultiplication()
        {
            Assert.AreEqual(sg.Add(this.t, this.f).Value, false);
            Assert.AreEqual(sg.Add(this.t, this.t).Value, true);
        }

        [Test]
        public void TestLessOrEqual()
        {
            Assert.IsTrue(this.sg.LessOrEqual(this.t, this.f));
            Assert.IsTrue(this.sg.LessOrEqual(this.t, this.t));
            Assert.IsTrue(this.sg.LessOrEqual(this.f, this.f));
        }
    }
}