using System;
using System.IO;

namespace MatrixMult
{
    public class MatrixReader
    {
        public static Matrix Reader(string path)
        {
            int[][] array;
            int M = 0;
            int N = 0;
            using (StreamReader sr = File.OpenText(path))
            {
                string line;
                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    var a = line.Split(' ');
                    N = a.Length;
                    i++;
                }

                M = i;
            }

            using (StreamReader sr = File.OpenText(path))
            {
                array = new int[M][];
                string line;
                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    var a = line.Split(' ');
                    array[i] = new int[N];
                    for (int j = 0; j < a.Length; j++)
                    {
                        array[i][j] = Int32.Parse(a[j]);
                    }
                    i++;
                }
            }

            return new Matrix(array);
        }
        
    }
}
