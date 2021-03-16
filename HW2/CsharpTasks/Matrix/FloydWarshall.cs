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
 
            var resArray = new T[matrix.Rows][];
            for (var i = 0; i < matrix.Rows; ++i)
            {
                resArray[i] = new T[matrix.Rows];
                for (var j = 0; j < matrix.Rows; ++j)
                {
                    resArray[i][j] = matrix.Array[i][j];
                    for (var k = 0; k < matrix.Rows; ++k)
                    {
                        var alternative = semigroup.Multiply(matrix.Array[i][k], matrix.Array[k][j]);
                        resArray[i][j] = semigroup.LessOrEqual(alternative, resArray[i][j])
                            ? alternative
                            : resArray[i][j];
                        matrix.Array[i][j] = resArray[i][j];
                    }
                }
            }

            return new Matrix<T>(resArray);
        }
    }
}