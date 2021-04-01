using Matrix;
using Matrix.AlgebraicStructures;
using NUnit.Framework;
using Boolean = Matrix.AlgebraicStructures.Boolean;

namespace Tests
{
    [TestFixture]
    public class FloydWarshallTest
    {
        private static Matrix<ExtendedReal> ToExtendedRealMatrix(string[][] floatTable)
        {
            var result = new ExtendedReal[floatTable.Length][];
            for (var i = 0; i < floatTable.Length; ++i)
            {
                result[i] = new ExtendedReal[floatTable[i].Length];
                for (var j = 0; j < floatTable[i].Length; ++j)
                {
                    result[i][j] = new ExtendedReal();
                    result[i][j].FromWord(floatTable[i][j]);
                }
            }

            return new Matrix<ExtendedReal>(result);
        }

        private static Matrix<Boolean> ToBoolMatrix(bool[][] boolTable)
        {
            var result = new Boolean[boolTable.Length][];
            for (var i = 0; i < boolTable.Length; ++i)
            {
                result[i] = new Boolean[boolTable[i].Length];
                for (var j = 0; j < boolTable[i].Length; ++j)
                    result[i][j] = new Boolean(boolTable[i][j]);
            }

            return new Matrix<Boolean>(result);
        }

        [Test]
        public void InfinityAllPairsShortestPathTest()
        {
            string[][] table1 =
                {
                    new[] {"0.0", "inf", "inf", "inf"},
                    new[] {"inf", "0.0", "inf", "inf"},
                    new[] {"inf", "inf", "0.0", "inf"},
                    new[] {"inf", "inf", "inf", "0.0"}
                },
                table2 =
                {
                    new[] {"0.0", "inf", "inf", "inf"},
                    new[] {"inf", "0.0", "inf", "inf"},
                    new[] {"inf", "inf", "0.0", "inf"},
                    new[] {"inf", "inf", "inf", "0.0"}
                };

            var origin = ToExtendedRealMatrix(table1);
            var expected = ToExtendedRealMatrix(table2);

            var result = FloydWarshall<ExtendedReal>.Execute(origin, new ExtendedRealSemigroup());

            for (var i = 0; i < expected.Columns; ++i)
                for (var j = 0; j < expected.Columns; ++j)
                    Assert.AreEqual(expected.Array[i][j].ToWord(), result.Array[i][j].ToWord());
        }
        
        
        
        [Test]
        public void AllPairsShortestPathTest()
        {
            string[][] table1 =
                {
                    new[] {"0.0", "inf", "-2.0", "inf"},
                    new[] {"4.0", "0.0", "3.0", "inf"},
                    new[] {"inf", "inf", "0.0", "2.0"},
                    new[] {"inf", "-1.0", "inf", "0.0"}
                },
                table2 =
                {
                    new[] {"0.0", "-1.0", "-2.0", "0.0"},
                    new[] {"4.0", "0.0", "2.0", "4.0"},
                    new[] {"5.0", "1.0", "0.0", "2.0"},
                    new[] {"3.0", "-1.0", "1.0", "0.0"}
                };

            var origin = ToExtendedRealMatrix(table1);
            var expected = ToExtendedRealMatrix(table2);

            var result = FloydWarshall<ExtendedReal>.Execute(origin, new ExtendedRealSemigroup());

            for (var i = 0; i < expected.Columns; ++i)
                for (var j = 0; j < expected.Columns; ++j)
                    Assert.AreEqual(expected.Array[i][j].ToWord(), result.Array[i][j].ToWord());
        }
        
        [Test]
        public void OneNodeAllPairsShortestPathTest()
        {
            string[][] table1 =
                {
                    new[] {"0.0"}
                },
                table2 =
                {
                    new[] {"0.0"}
                };

            var origin = ToExtendedRealMatrix(table1);
            var expected = ToExtendedRealMatrix(table2);

            var result = FloydWarshall<ExtendedReal>.Execute(origin, new ExtendedRealSemigroup());

            for (var i = 0; i < expected.Columns; ++i)
                for (var j = 0; j < expected.Columns; ++j)
                    Assert.AreEqual(expected.Array[i][j].ToWord(), result.Array[i][j].ToWord());
        }

        [Test]
        public void TransitiveClosureTest()
        {
            bool[][] array0 =
            {
                new[] {true, true, false, false},
                new[] {false, true, true, false},
                new[] {true, false, true, false},
                new[] {false, false, true, true}
            };

            bool[][] array1 =
            {
                new[] {true, true, true, false},
                new[] {true, true, true, false},
                new[] {true, true, true, false},
                new[] {true, true, true, true}
            };

            var origin = ToBoolMatrix(array0);
            var expected = ToBoolMatrix(array1);

            var result = FloydWarshall<Boolean>.Execute(origin, new BooleanSemigroup());

            for (var i = 0; i < expected.Columns; i++)
                for (var j = 0; j < expected.Columns; j++)
                    Assert.AreEqual(expected.Array[i][j].Value, result.Array[i][j].Value);
        }
        
        [Test]
        public void FalseTransitiveClosureTest()
        {
            bool[][] array0 =
            {
                new[] {true, false, false, false},
                new[] {false, true, false, false},
                new[] {false, false, true, false},
                new[] {false, false, false, true}
            };

            bool[][] array1 =
            {
                new[] {true, false, false, false},
                new[] {false, true, false, false},
                new[] {false, false, true, false},
                new[] {false, false, false, true}
            };

            var origin = ToBoolMatrix(array0);
            var expected = ToBoolMatrix(array1);

            var result = FloydWarshall<Boolean>.Execute(origin, new BooleanSemigroup());

            for (var i = 0; i < expected.Columns; i++)
                for (var j = 0; j < expected.Columns; j++)
                    Assert.AreEqual(expected.Array[i][j].Value, result.Array[i][j].Value);
        }
        
        
        [Test]
        public void OneNodeTransitiveClosureTest()
        {
            bool[][] array0 =
            {
                new[] {true}
            };

            bool[][] array1 =
            {
                new[] {true}
            };

            var origin = ToBoolMatrix(array0);
            var expected = ToBoolMatrix(array1);

            var result = FloydWarshall<Boolean>.Execute(origin, new BooleanSemigroup());

            for (var i = 0; i < expected.Columns; i++)
                for (var j = 0; j < expected.Columns; j++)
                    Assert.AreEqual(expected.Array[i][j].Value, result.Array[i][j].Value);
        }
    }
}