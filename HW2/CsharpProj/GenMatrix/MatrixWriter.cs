﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsharpProj
{
    public class MatrixWriter<T> where T : ISerializable
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
    }
}
