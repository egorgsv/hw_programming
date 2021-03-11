using System;

namespace Matrix
{
    public class Program
    {
        static void Main(string[] args)
        {
            String path1 = args[0],
                path2 = args[1],
                output = args[2];
            Natural[][] array1 = MatrixIO<Natural>.Reader(path1);
            Natural[][] array2 = MatrixIO<Natural>.Reader(path2);
            try
            {
                Matrix<Natural> matrix1 = new Matrix<Natural>(array1);
                Matrix<Natural> matrix2 = new Matrix<Natural>(array2);
                Matrix<Natural> matrix = Matrix<Natural>.Multiply(matrix1, matrix2, new NaturalSemiring());
                MatrixIO<Natural>.WriteMatrix(matrix.array, output);
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}