using System;
using Matrix.Interfaces;

namespace Matrix
{
    public class FloydWarshall<T>
    {
        public static Matrix<T> Execute(Matrix<T> mtrx, ISemigroupPO<T> semigroup)
        {
            var matrix = new Matrix<T>(mtrx.Copy());
            if (matrix.Rows != matrix.Columns)
                throw new ArgumentException("Matrix should be square.");
            
            for (var k = 0; k < matrix.Rows; k++)
            {
                for (var i = 0; i < matrix.Rows; i++)
                {
                    for (var j = 0; j < matrix.Rows; j++)
                    {
                        var alternative = semigroup.Add(matrix.Array[i][k], matrix.Array[k][j]);
                        matrix.Array[i][j] = semigroup.LessOrEqual(alternative, matrix.Array[i][j])
                            ? alternative
                            : matrix.Array[i][j];
                    }
                }
            }

            return new Matrix<T>(matrix.Array);
        }
    }
}