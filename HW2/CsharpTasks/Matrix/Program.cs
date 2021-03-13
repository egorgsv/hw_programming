using System;
using Matrix.AlgebraicStructures;

namespace Matrix
{
    public static class Program
    {
        static void Main(string[] args)
        {
            string path1 = args[0],
                path2 = args[1],
                output = args[2];
            var array1 = MatrixIO<Natural>.Reader(path1);
            var array2 = MatrixIO<Natural>.Reader(path2);
            try
            {
                var matrix1 = new Matrix<Natural>(array1);
                var matrix2 = new Matrix<Natural>(array2);
                var matrix = Matrix<Natural>.Multiply(matrix1, matrix2, new NaturalSemiring());
                MatrixIO<Natural>.WriteMatrix(matrix.Array, output);
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}