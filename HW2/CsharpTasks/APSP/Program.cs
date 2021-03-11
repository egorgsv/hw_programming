﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Matrix;

namespace APSP
{
    public class Program
    {
        static void Main(string[] args)
        {
            String path = args[0],
                output = args[1];
            Natural[][] array = MatrixIO<Natural>.Reader(path);

            try
            {
                Matrix<Natural> matrix = new Matrix<Natural>(array);
                Matrix<Natural> APSPMatrix = FloydWarshall<Natural>.Execute(matrix, new NaturalSemigroup());
                MatrixIO<Natural>.WriteMatrix(APSPMatrix.array, output);
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}