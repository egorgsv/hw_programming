using System;
using CsharpProj;

namespace Matrix
{
    public class Program
    {
        static void Main(string[] args)
        {
            String path1 = Console.ReadLine(),
                path2 = Console.ReadLine(),
                output = Console.ReadLine();
            Natural[][] array1 = MatrixReader<Natural>.Reader(path1);
            Natural[][] array2 = MatrixReader<Natural>.Reader(path2);
            try
            {
                Matrix<Natural> matrix1 = new Matrix<Natural>(array1);
                Matrix<Natural> matrix2 = new Matrix<Natural>(array2);
                Matrix<Natural> matrix = Matrix<Natural>.Multiply(matrix1, matrix2, new NaturalSemiring());
                MatrixWriter<Natural>.WriteMatrix(matrix.array, output);
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
