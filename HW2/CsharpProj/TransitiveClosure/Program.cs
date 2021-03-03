using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using CsharpProj;

namespace TransitiveClosure
{
    public class Program
    {
        static void Main(string[] args)
        {
            String path = "/Users/egorgusev/Programming/hw_programming/HW2/CsharpProj/Tests/test_data/bool.txt";
            //String path = Console.ReadLine(),
                //output = Console.ReadLine();
            Boolean[][] array = MatrixReader<Boolean>.Reader(path);

            try
            {
                Matrix<Boolean> mtrx = new Matrix<Boolean>(array);
                Matrix<Boolean> transClosureMtrx = FloydWarshall<Boolean>.Execute(mtrx, new BooleanSemigroup());

                String GetDotCode(Boolean[][] matrix, Boolean[][] transClosureMatrix)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine("digraph G {");
                    for (int i = 0; i < transClosureMatrix.Length; ++i)
                        builder.Append(i.ToString());
                    builder.AppendLine(";");
                    for (int i = 0; i < transClosureMatrix.Length; ++i)
                        for (int j = 0; j < transClosureMatrix[0].Length; ++j)
                            if (transClosureMatrix[i][j].value)
                                builder.Append(i.ToString() + " -> " + j.ToString() + " [" + (matrix[i][j].value ? "" : "color=red") + "];");
                    builder.AppendLine(";");
                    return builder.ToString();

                }

                void GeneratePDF(String res, String dot)
                {
                    String dotFile = res + ".dot";
                    using (StreamWriter writer = File.CreateText(dotFile))
                        writer.Write(dot);
                    using (Process process = new Process())
                    {
                        process.StartInfo.FileName = "dot";
                        process.StartInfo.Arguments = "-Tpdf -o" + res + " " + dotFile;
                        process.Start();
                        while (!process.HasExited)
                            process.Refresh();
                    }
                    File.Delete(dotFile);
                }

                string outputDot = GetDotCode(mtrx.array, transClosureMtrx.array);
                GeneratePDF("graphVis.pdf", outputDot);
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
