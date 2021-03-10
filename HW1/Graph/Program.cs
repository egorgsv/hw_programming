using System;
using MatrixMult;
using System.Collections.Generic;
using System.Data;
using System.IO;
using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Algorithms.ShortestPath;
using QuickGraph.Collections;
using QuickGraph.Data;
using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;
using System.Text;
using System.Diagnostics;

namespace Graph
{
    class Program
    {
        static void Main(string[] args)
        {
            String path = args[0],
                output = args[1];
            var adjacencyMatrix = MatrixReader.Reader(path);

            (BidirectionalMatrixGraph<Edge<int>> graph,
                    Func<Edge<int>, double> edgeWeights, int vertexCount) = new Graph(adjacencyMatrix).GetResult();

            int root = 0;

            TryFunc<int, IEnumerable<Edge<int>>> tryGetPaths = graph.ShortestPathsDijkstra(edgeWeights, root);

            var graphShort = new BidirectionalMatrixGraph<Edge<int>>(vertexCount);

            // query path for given vertices
            foreach (var target in graph.Vertices)
            {
                IEnumerable<Edge<int>> pathShort;
                if (tryGetPaths(target, out pathShort))
                    foreach (var edge in pathShort)
                        if (!graphShort.ContainsEdge(edge))
                        {
                            graphShort.AddEdge(edge);
                        }
            }

            // render
            String GetDotCode(BidirectionalMatrixGraph<Edge<int>> shortestTree, BidirectionalMatrixGraph<Edge<int>> graph)
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("digraph G {");
                foreach (var vertex in graph.Vertices)
                {
                    builder.Append(vertex.ToString());
                    builder.AppendLine(";");
                }
                foreach (var edge in graph.Edges)
                {
                    builder.Append(edge.ToString());
                    if (shortestTree.ContainsEdge(edge))
                        builder.AppendLine(" [color = red];");
                    else
                        builder.AppendLine(" [];");
                }
                builder.AppendLine("}");
                return builder.ToString();
            }

            void GeneratePDF(String output, String dot)
            {
                String dotFile = output + ".dot";
                using (StreamWriter writer = File.CreateText(dotFile))
                    writer.Write(dot);
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = "dot";
                    process.StartInfo.Arguments = "-Tpdf -o" + output + " " + dotFile;
                    process.Start();
                    while (!process.HasExited)
                        process.Refresh();
                }
                File.Delete(dotFile);
            }

            string outputDot = GetDotCode(graphShort, graph);
            GeneratePDF(output, outputDot);

        }
    }
}
