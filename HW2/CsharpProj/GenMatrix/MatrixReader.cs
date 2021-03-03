﻿using System;
using System.IO;

namespace CsharpProj
{
    public class MatrixReader<T> where T : ISerializable, new()
    {
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
