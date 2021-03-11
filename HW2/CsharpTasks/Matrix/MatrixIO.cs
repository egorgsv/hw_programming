﻿using System;
using System.IO;
 using System.Text;

 namespace Matrix 
 {
    public class MatrixIO<T> where T : ISerializable, new()
    {
        
        public static void WriteMatrix(T[][] matrix, String output)
        {
            StringBuilder builder = new StringBuilder();
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
                array = new T[M][];
                string line;
                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    var a = line.Split(' ');
                    array[i] = new T[N];
                    for (int j = 0; j < a.Length; j++)
                    {
                        T t = new T();
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
