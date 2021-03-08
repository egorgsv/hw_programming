﻿using System;
namespace CsharpProj
{
    public class FloydWarshall<T>
    {
        public static Matrix<T> Execute(Matrix<T> mtrx, ISemigroupPO<T> semigroup)
        {
            Matrix<T> matrix = new Matrix<T>(mtrx.Copy());
            if (matrix.n != matrix.m)
                throw new ArgumentException("Matrix should be square.");
            T[][] resArray = new T[matrix.n][];
            for (int i = 0; i < matrix.n; ++i)
            {
                resArray[i] = new T[matrix.n];
                for (int j = 0; j < matrix.n; ++j)
                {
                    resArray[i][j] = matrix.array[i][j];
                    for (int k = 0; k < matrix.n; ++k)
                    {
                        T alternative = semigroup.Multiply(matrix.array[i][k], matrix.array[k][j]);
                        resArray[i][j] = semigroup.LessOrEqual(alternative, resArray[i][j]) ? alternative : resArray[i][j];
                        matrix.array[i][j] = resArray[i][j];
                    }
                }
            }
            return new Matrix<T>(resArray);
        }
    }
}
