using System;
using CsharpProj;
using Matrix;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class MatrixMultiplierTest
    {
        private Matrix<Natural> toNaturlMatrix(int[][] intTable) {
            Natural[][] result = new Natural[intTable.Length][];
            int i, j;
            for (i = 0; i < intTable.Length; ++i) {
                result[i] = new Natural[intTable[i].Length];
                for (j = 0; j < intTable[i].Length; ++j)
                    if (intTable[i][j] > 0)
                        result[i][j] = new Natural((uint)intTable[i][j]);
                    else throw new ArgumentException();
            }
            return new Matrix<Natural>(result);
        }

        [Test]
        public void TestCommonCase() {
            int[][] table1 = {
                new [] {1, 2, 3},
                new [] {4, 5, 6},
                new [] {7, 8, 9}
            },
            table2 = {
                new [] {1, 4},
                new [] {2, 5},
                new [] {3, 6}
            };
            Matrix<Natural> matrix1 = toNaturlMatrix(table1),
                matrix2 = toNaturlMatrix(table2);
            Matrix<Natural> matrix = Matrix<Natural>.Multiply(matrix1, matrix2, new NaturalSemiring());
            int[][] expected = {
                new [] {14, 32},
                new [] {32, 77},
                new [] {50, 122}
            };
            int i, j;
            for (i = 0; i < expected.Length; ++i)
                for (j = 0; j < expected[0].Length; ++j)
                    Assert.AreEqual(matrix.array[i][j].value, expected[i][j]);
        }

        [Test]
        public void TestWrongSizes() {
            int[][] t1 = {
                new [] {1, 2, 3},
                new [] {4, 5, 6}
            },
            t2 = {
                new [] {1, 2, 3},
                new [] {4, 5, 6},
                new [] {7, 8, 9}
            };
            Matrix<Natural> matrix1 = this.toNaturlMatrix(t1),
                matrix2 = this.toNaturlMatrix(t2);
            Assert.Throws<ArgumentException>(
                delegate {
                    Matrix<Natural>.Multiply(matrix2, matrix1, new NaturalSemiring());
                });
        }

        [Test]
        public void TestEmptyMatrixCreation()
        {
            int[][] invalid0 = {};
            int[][] invalid1 = {
                new int[] { },
                new int[] { },
                new int[] { }
            };

            Assert.Throws<ArgumentException>(
                delegate {
                    Matrix<Natural> m = toNaturlMatrix(invalid0);
                }
            );
            Assert.Throws<ArgumentException>(
                delegate {
                    Matrix<Natural> m = toNaturlMatrix(invalid1);
                }
            );
        }

        [Test]
        public void TestNotRectangleMatrixCreation()
        {
            int[][] invalid = {
                new int[] {1, 2, 3, 4},
                new int[] {1, 2, 3},
                new int[] {1, 2, 3, 4},
            };
            Assert.Throws<ArgumentException>(
                delegate {
                    Matrix<Natural> m = toNaturlMatrix(invalid);
                }
            );
        }
        
        [Test]
        public void TestInvalidMatrixElement()
        {
            int[][] t = {
                new int[] {1, -1},
                new int[] {1, 2}
            };
            Assert.Throws<ArgumentException>(
                delegate {
                    Matrix<Natural> m = toNaturlMatrix(t);
                });
        }
    }
}