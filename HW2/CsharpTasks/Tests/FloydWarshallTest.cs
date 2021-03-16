using Matrix;
using Matrix.AlgebraicStructures;
using NUnit.Framework;
using Boolean = Matrix.AlgebraicStructures.Boolean;

namespace Tests
{
    [TestFixture]
    public class FloydWarshallTest
    {
        private static Matrix<Natural> ToNaturalMatrix(int[][] intTable)
        {
            var result = new Natural[intTable.Length][];
            for (var i = 0; i < intTable.Length; ++i)
            {
                result[i] = new Natural[intTable[i].Length];
                for (var j = 0; j < intTable[i].Length; ++j)
                    result[i][j] = new Natural((uint) intTable[i][j]);
            }

            return new Matrix<Natural>(result);
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
        public void AllPairsShortestPathTest()
        {
            int[][] table1 =
                {
                    new[] {0, 1, 4},
                    new[] {6, 0, 1},
                    new[] {1, 5, 0}
                },
                table2 =
                {
                    new[] {0, 1, 2},
                    new[] {2, 0, 1},
                    new[] {1, 2, 0}
                };

            var origin = ToNaturalMatrix(table1);
            var expected = ToNaturalMatrix(table2);

            var result = FloydWarshall<Natural>.Execute(origin, new NaturalSemigroup());

            for (var i = 0; i < expected.Columns; ++i)
            for (var j = 0; j < expected.Columns; ++j)
                Assert.AreEqual(expected.Array[i][j].Value, result.Array[i][j].Value);
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

            for (var i = 0; i < expected.Columns; ++i)
            for (var j = 0; j < expected.Columns; ++j)
                Assert.AreEqual(expected.Array[i][j].Value, result.Array[i][j].Value);
        }
    }
}