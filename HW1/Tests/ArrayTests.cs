using System;
using NUnit.Framework;
using ResizeArray;

namespace Tests
{
    [TestFixture]
    public class ResizeArrayTest
    {
        private ResizeArray<int> array;
        private Random random;

        private void setUp()
        {
            array = new ResizeArray<int>();
            for (int i = 0; i < 50; i++)
                array.AddTail(i);
        }

        public ResizeArrayTest()
        {
            setUp();
            random = new Random();
        }

        [TearDown]
        public void Cleanup() => setUp();

        [Test]
        public void TestGetElements()
        {
            int index;
            for (int i = 0; i < 50;  i++)
            {
                index = random.Next(0, 50);
                Assert.AreEqual(index, array[index]);
            }
        }

        [Test]
        public void TestSetElements()
        {
            array[0] = 1234;
            Assert.AreEqual(array[0], 1234);
        }

        [Test]
        public void TestOutOfRange()
        {
            int i, x;
            for (i = 0; i < 20; i++)
            {
                Assert.Throws<IndexOutOfRangeException>(
                    delegate {
                        x = array[-random.Next()];
                    }
                );
            }

            for (i = 0; i < 20; i++)
            {
                Assert.Throws<IndexOutOfRangeException>(
                    delegate {
                        x = array[50 + random.Next()];
                    }
                );
            }
        }
    }
}