using System;
using Matrix;
using Matrix.AlgebraicStructures;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class MatrixMultTest
    {
        private static Matrix<Natural> ToNaturalMatrix(int[][] intTable)
        {
            var result = new Natural[intTable.Length][];
            for (var i = 0; i < intTable.Length; ++i)
            {
                result[i] = new Natural[intTable[i].Length];
                for (var j = 0; j < intTable[i].Length; ++j)
                    if (intTable[i][j] > 0)
                        result[i][j] = new Natural((uint) intTable[i][j]);
                    else throw new ArgumentException();
            }

            return new Matrix<Natural>(result);
        }

        [Test]
        public void TestCommonCase()
        {
            int[][] table1 =
                {
                    new[] {1, 2, 3},
                    new[] {4, 5, 6},
                    new[] {7, 8, 9}
                },
                table2 =
                {
                    new[] {1, 4},
                    new[] {2, 5},
                    new[] {3, 6}
                };
            Matrix<Natural> matrix1 = ToNaturalMatrix(table1),
                matrix2 = ToNaturalMatrix(table2);
            var matrix = Matrix<Natural>.Multiply(matrix1, matrix2, new NaturalSemiring());
            int[][] expected =
            {
                new[] {14, 32},
                new[] {32, 77},
                new[] {50, 122}
            };
            for (var i = 0; i < expected.Length; ++i)
            for (var j = 0; j < expected[0].Length; ++j)
                Assert.AreEqual(matrix.Array[i][j].Value, expected[i][j]);
        }

        [Test]
        public void TestWrongSizes()
        {
            int[][] t1 =
                {
                    new[] {1, 2, 3},
                    new[] {4, 5, 6}
                },
                t2 =
                {
                    new[] {1, 2, 3},
                    new[] {4, 5, 6},
                    new[] {7, 8, 9}
                };
            Matrix<Natural> matrix1 = ToNaturalMatrix(t1),
                matrix2 = ToNaturalMatrix(t2);
            Assert.Throws<ArgumentException>(
                delegate { Matrix<Natural>.Multiply(matrix2, matrix1, new NaturalSemiring()); });
        }

        [Test]
        public void TestEmptyMatrixCreation()
        {
            int[][] invalid0 = { };
            int[][] invalid1 =
            {
                new int[] { },
                new int[] { },
                new int[] { }
            };

            Assert.Throws<ArgumentException>(
                delegate { ToNaturalMatrix(invalid0); }
            );
            Assert.Throws<ArgumentException>(
                delegate { ToNaturalMatrix(invalid1); }
            );
        }

        [Test]
        public void TestNotRectangleMatrixCreation()
        {
            int[][] invalid =
            {
                new[] {1, 2, 3, 4},
                new[] {1, 2, 3},
                new[] {1, 2, 3, 4},
            };
            Assert.Throws<ArgumentException>(
                delegate { ToNaturalMatrix(invalid); }
            );
        }

        [Test]
        public void TestInvalidMatrixElement()
        {
            int[][] t =
            {
                new[] {1, -1},
                new[] {1, 2}
            };
            Assert.Throws<ArgumentException>(
                delegate { ToNaturalMatrix(t); });
        }
    }
}