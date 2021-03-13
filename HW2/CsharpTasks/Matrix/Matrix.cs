using System;
using Matrix.Interfaces;

namespace Matrix
{
    public class Matrix<T>
    {
        public T[][] Array;
        public int Rows { get; private set; }
        public int Columns { get; private set; }

        public Matrix(T[][] array)
        {
            var rowsCount = array.Length;
            if (rowsCount == 0)
                throw new ArgumentException("Matrix should contain at least one cell.");

            var columnsCount = array[0].Length;
            if (columnsCount == 0)
                throw new ArgumentException("Matrix should contain at least one cell.");

            foreach (var row in array)
                if (row.Length != columnsCount)
                    throw new ArgumentException("Rows' lengths should be equal.");

            this.Rows = rowsCount;
            this.Columns = columnsCount;
            this.Array = array;
        }

        public static Matrix<T> Multiply(Matrix<T> matrix1, Matrix<T> matrix2, ISemiring<T> semiring)
        {
            var n = matrix1.Rows;
            var m = matrix2.Columns;
            if (matrix1.Columns != matrix2.Rows)
                throw new ArgumentException(
                    "Column count of first matrix should equal to rows count of the second one.");

            var resArray = new T[n][];

            for (var i = 0; i < n; i++)
            {
                var row = new T[m];
                for (var j = 0; j < m; j++)
                {
                    row[j] = semiring.GetIdentityElement();
                    for (var o = 0; o < matrix1.Columns; o++)
                    {
                        row[j] = semiring.Add(row[j], semiring.Multiply(matrix1.Array[i][o], matrix2.Array[o][j]));
                    }

                    resArray[i] = row;
                }
            }

            return new Matrix<T>(resArray);
        }

        public T[][] Copy()
        {
            var copy = new T[this.Rows][];
            for (var i = 0; i < copy.Length; ++i)
                copy[i] = (T[]) this.Array[i].Clone();

            return copy;
        }

        public T[][] GetContent() => (T[][]) Array.Clone();
    }
}