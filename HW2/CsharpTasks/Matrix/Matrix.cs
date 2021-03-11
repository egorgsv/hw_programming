using System;

namespace Matrix
 {
     public class Matrix<T>
    {
        public T[][] array;
        public int n { get; private set; }
        public int m { get; private set; }

        public Matrix(T[][] array)
        {
            int n = array.Length;
            isZero(n);
            int m = array[0].Length;
            isZero(m);
            foreach (T[] row in array)
                if (row.Length != m)
                    throw new ArgumentException("Rows' lengths should be equal.");
            this.n = n;
            this.m = m;
            this.array = array;
        }

        private void isZero(int norm)
        {
            if (norm == 0)
                throw new ArgumentException("Matrix should contain at least one cell.");
        }

        public static Matrix<T> Multiply(Matrix<T> matrix1, Matrix<T> matrix2, ISemiring<T> semiring)
        {
            int n = matrix1.n;
            int m = matrix2.m;
            if (matrix1.m != matrix2.n)
                throw new ArgumentException("Colum count of first matrix should equal to rows count of the second one.");

            T[][] resArray = new T[n][];

            for (int i = 0; i < n; i++)
            {
                T[] row = new T[m];
                for (int j = 0; j < m; j++)
                {
                    row[j] = semiring.GetIdentityElement();
                    for (int o = 0; o < matrix1.m; o++)
                    {
                        row[j] = semiring.Add(row[j], semiring.Multiply(matrix1.array[i][o], matrix2.array[o][j]));
                    }
                    resArray[i] = row;
                }
            }

            return new Matrix<T>(resArray);
        }

        public T[][] Copy()
        {
            T[][] copy = new T[this.n][];
            for (int i = 0; i < copy.Length; ++i)
                copy[i] = (T[])this.array[i].Clone();
            return copy;
        }

        public T[][] GetContent() => (T[][])array.Clone();
    }
}
