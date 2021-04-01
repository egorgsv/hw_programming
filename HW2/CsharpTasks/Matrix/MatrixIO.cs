using System;
using System.IO;
using System.Text;
using Matrix.Interfaces;

namespace Matrix
{
    public class MatrixIO<T> where T : ISerializable, new()
    {
        public static void WriteMatrix(T[][] matrix, String output)
        {
            var builder = new StringBuilder();
            foreach (var line in matrix)
            {
                foreach (var i in line)
                {
                    builder.Append(i.ToWord());
                    builder.Append(' ');
                }

                builder.AppendLine();
            }

            File.WriteAllText(output, builder.ToString());
        }

        public static T[][] Reader(string path)
        {
            T[][] array;
            int rowCount;
            var columnCount = 0;
            using (var sr = File.OpenText(path))
            {
                string line;
                var i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    var a = line.Split(' ');
                    columnCount = a.Length;
                    i++;
                }

                rowCount = i;
            }

            using (var sr = File.OpenText(path))
            {
                array = new T[rowCount][];
                string line;
                var i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    var a = line.Split(' ');
                    array[i] = new T[columnCount];
                    for (var j = 0; j < a.Length; j++)
                    {
                        var t = new T();
                        t.FromWord(a[j]);
                        array[i][j] = t;
                    }

                    i++;
                }
            }

            return array;
        }
    }
}