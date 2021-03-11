using System;
using Matrix;
using NUnit.Framework;
using Boolean = Matrix.Boolean;

namespace FloydWarshallTest
{
    [TestFixture]
    public class FloydWarshallTest
    {
        private Matrix<Natural> toNaturalMatrix(int[][] intTable) {
            Natural[][] result = new Natural[intTable.Length][];
            int i, j;
            for (i = 0; i < intTable.Length; ++i) {
                result[i] = new Natural[intTable[i].Length];
                for (j = 0; j < intTable[i].Length; ++j)
                    result[i][j] = new Natural((uint)intTable[i][j]);
            }
            return new Matrix<Natural>(result);
        }
        
        private Matrix<Boolean> toBoolMatrix(bool[][] boolTable) {
            Boolean[][] result = new Boolean[boolTable.Length][];
            for (int i = 0; i < boolTable.Length; ++i) {
                result[i] = new Boolean[boolTable[i].Length];
                for (int j = 0; j < boolTable[i].Length; ++j)
                    result[i][j] = new Boolean(boolTable[i][j]);
            }
            return new Matrix<Boolean>(result);
        }
        
        [Test]
        public void AllPairsShortestPathTest()
        {
            int[][] table1 = {
                    new [] {0, 1, 4},
                    new [] {6, 0, 1},
                    new [] {1, 5, 0}
                },
                table2 = {
                    new [] {0, 1, 2},
                    new [] {2, 0, 1},
                    new [] {1, 2, 0}
                };

            Matrix<Natural> origin = toNaturalMatrix(table1);
            Matrix<Natural> expected = toNaturalMatrix(table2);

            Matrix<Natural> result = FloydWarshall<Natural>.Execute(origin, new NaturalSemigroup());

            for (int i = 0; i < expected.m; ++i)
                for (int j = 0; j < expected.m; ++j)
                    Assert.AreEqual(expected.array[i][j].value, result.array[i][j].value);
        }

        [Test]
        public void TransitiveClousureTest()
        {
            bool[][] array0 = { 
                new [] { true, true, false, false },
                new [] { false, true, true, false },
                new [] { true, false, true, false },
                new [] { false, false, true, true } 
            };

            bool[][] array1 = { 
                new [] { true, true, true, false },
                new [] { true, true, true, false },
                new [] { true, true, true, false },
                new [] { true, true, true, true }
            };

            Matrix<Boolean> origin = toBoolMatrix(array0);
            Matrix<Boolean> expected = toBoolMatrix(array1);

            Matrix<Boolean> result = FloydWarshall<Boolean>.Execute(origin, new BooleanSemigroup());

            for (int i = 0; i < expected.m; ++i)
                for (int j = 0; j < expected.m; ++j)
                    Assert.AreEqual(expected.array[i][j].value, result.array[i][j].value);
        }
    }
}