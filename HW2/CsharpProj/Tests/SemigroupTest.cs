using System.Diagnostics;
using NUnit.Framework;
using TransitiveClosure;

namespace Tests
{
    [TestFixture]
    class SemigroupTest {

        private BooleanSemigroup sg;
        private Boolean t, f;

        public SemigroupTest() {
            sg = new BooleanSemigroup();
            f = new Boolean(false);
            t = new Boolean(true);
        }

        [Test]
        public void TestMultiplication() {
            Assert.AreEqual(sg.Multiply(this.t, this.f).value, false);
            Assert.AreEqual(sg.Multiply(this.t, this.t).value, true);
        }

        [Test]
        public void TestLessOrEqual() {
            Assert.IsTrue(this.sg.LessOrEqual(this.t, this.f));
            Assert.IsTrue(this.sg.LessOrEqual(this.t, this.t));
            Assert.IsTrue(this.sg.LessOrEqual(this.f, this.f));
        }
    }
}