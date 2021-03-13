using System.IO;
using System.Text;
using System.Diagnostics;
using Boolean = Matrix.AlgebraicStructures.Boolean;

namespace TransitiveClosure
{
    public static class PdfGenerator
    {
        public static string GetDotCode(Boolean[][] matrix, Boolean[][] transClosureMatrix)
        {
            var builder = new StringBuilder();
            builder.AppendLine("digraph G {");
            for (var i = 0; i < transClosureMatrix.Length; ++i)
            for (var j = 0; j < transClosureMatrix[0].Length; ++j)
                if (transClosureMatrix[i][j].Value)
                    builder.Append(i + " -> " + j + " [" + (matrix[i][j].Value ? "" : "color=red") + "];");

            builder.AppendLine("}");
            return builder.ToString();
        }

        public static void GeneratePDF(string res, string dot)
        {
            var dotFile = res + ".dot";
            using (var writer = File.CreateText(dotFile))
                writer.Write(dot);
            using (var process = new Process())
            {
                process.StartInfo.FileName = "dot";
                process.StartInfo.Arguments = "-Tpdf -o" + res + " " + dotFile;
                process.Start();
                while (!process.HasExited)
                    process.Refresh();
            }

            File.Delete(dotFile);
        }
    }
}