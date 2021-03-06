﻿using System;
using System.Text;

namespace MatrixMult
{
    public class Matrix
    {
        public int[][] array;
        public int n { get; private set; }
        public int m { get; private set; }

        public Matrix(int[][] array)
        {
            int n = array.Length;
            if (n == 0)
                throw new ArgumentException("Matrix should contain at least one cell.");
            int m = array[0].Length;
            if (m == 0)
                throw new ArgumentException("Matrix should contain at least one cell.");
            foreach (int[] row in array)
                if (row.Length != m)
                    throw new ArgumentException("Rows' lengths should be equal.");
            this.n = n;
            this.m = m;
            this.array = array;
        }

        public static Matrix operator *(Matrix matrix1, Matrix matrix2)
        {
            int n = matrix1.n;
            int m = matrix2.m;
            if (matrix1.m != matrix2.n)
                throw new ArgumentException("Colum count of first matrix should equal to rows count of the second one.");

            int[][] resArray = new int[n][];

            for (int i = 0; i < n; i++)
            {
                resArray[i] = new int[m];
                for (int j = 0; j < m; j++)
                {
                    resArray[i][j] = 0;
                    for (int o = 0; o < matrix1.m; o++)
                    {
                        resArray[i][j] += matrix1.array[i][o] * matrix2.array[o][j];
                    }
                }
            }

            return new Matrix(resArray);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var line in this.array)
            {
                foreach (var i in line)
                {
                    builder.Append(i.ToString());
                    builder.Append(' ');
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;
            var matrix = obj as Matrix;
            if (matrix.m != this.m || matrix.n != this.n)
            {
                return false;
            }
            for (int i = 0; i < this.n; i++)
            {
                for (int j = 0; j < this.m; j++)
                {
                    if (matrix.array[i][j] != this.array[i][j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public int[][] GetContent() => (int[][])this.array.Clone();

        public override int GetHashCode() => array.GetHashCode();
    }
}
