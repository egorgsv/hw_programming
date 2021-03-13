using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using Matrix;
using Matrix.AlgebraicStructures;
using static TransitiveClosure.PdfGenerator;
using Boolean = Matrix.AlgebraicStructures.Boolean;

namespace TransitiveClosure
{
    public static class Program
    {
        static void Main(string[] args)
        {
            string path, output;

            try
            {
                path = args[0];
                output = args[1];
            }
            catch (IndexOutOfRangeException err)
            {
                Console.WriteLine("Error: expected two arguments -- input and output path\n");
                throw new ArgumentException(err.Message);
            }

            try
            {
                var array = MatrixIO<Boolean>.Reader(path);
                var inputMatrix = new Matrix<Boolean>(array);
                var transClosureMatrix = FloydWarshall<Boolean>.Execute(inputMatrix, new BooleanSemigroup());
                var outputDot = GetDotCode(inputMatrix.Array, transClosureMatrix.Array);
                GeneratePDF(output, outputDot);
                MatrixIO<Boolean>.WriteMatrix(transClosureMatrix.Array, output);
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}