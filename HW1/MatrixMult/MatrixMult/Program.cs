using System;

namespace MatrixMult
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                String path1 = args[0],
                    path2 = args[1];
                Matrix matrix1 = MatrixReader.Reader(path1),
                    matrix2 = MatrixReader.Reader(path2);
                Console.WriteLine((matrix1 * matrix2).ToString());
            }
            catch (IndexOutOfRangeException exn)
            {
                Console.Error.WriteLine("Paths to factors should be given.");
            }
        }
    }
}
