using System;
using Matrix;
using Matrix.AlgebraicStructures;

namespace APSP
{
    public static class Program
    {
        static void Main(string[] args)
        {
            string path = args[0],
                output = args[1];
            var array = MatrixIO<Natural>.Reader(path);

            try
            {
                var matrix = new Matrix<Natural>(array);
                var apspMatrix = FloydWarshall<Natural>.Execute(matrix, new NaturalSemigroup());
                MatrixIO<Natural>.WriteMatrix(apspMatrix.Array, output);
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}