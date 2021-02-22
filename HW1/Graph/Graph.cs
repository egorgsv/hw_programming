using System;
using System.Collections.Generic;
using MatrixMult;
using QuickGraph;
using QuickGraph.Algorithms;

namespace Graph
{
    public class Graph
    {
        public BidirectionalMatrixGraph<Edge<int>> graph;
        List<Edge<int>> edges;
        public Func<Edge<int>, double> edgeWeights;
        int[][] adjacencyMatrix;
        public int vertexCount;

        public Graph(Matrix matrix)
        {
            if (matrix.m != matrix.n)
                throw new ArgumentException("Не квадратная матрица");
            adjacencyMatrix = matrix.GetContent();
            graph = null;
            edges = null;
            edgeWeights = null;
            vertexCount = matrix.n;
        }

        public (BidirectionalMatrixGraph<Edge<int>>, Func<Edge<int>, double>, int) GetResult()
        {
            if (this.graph != null)
                return (this.graph, this.edgeWeights, vertexCount);
            var graph = new BidirectionalMatrixGraph<Edge<int>>(vertexCount);
            var edges = new List<Edge<int>>();
            for (int i = 0; i < this.vertexCount; i++)
            {
                for (int j = 0; j < this.vertexCount; j++)
                {
                    if (i != j && adjacencyMatrix[i][j] > 0)
                    {
                        edges.Add(new Edge<int>(i, j));
                    }
                }
            }

            graph.AddEdgeRange(edges);
            this.edgeWeights = edge => adjacencyMatrix[edge.Source][edge.Target];
            this.graph = graph;
            return GetResult();
        }

        //public BidirectionalMatrixGraph<Edge<int>> FindShortestPaths(int root)
        //{
        //    TryFunc<int, IEnumerable<Edge<int>>> tryGetPaths = graph.ShortestPathsDijkstra(edgeWeights, root);
            
        //    var graphShort = new BidirectionalMatrixGraph<Edge<int>>(vertexCount);
            
        //    // query path for given vertices
        //    foreach (var target in graph.Vertices)
        //    {
        //        IEnumerable<Edge<int>> pathShort;
        //        if (tryGetPaths(target, out pathShort))
        //            foreach (var edge in pathShort)
        //                if (!graphShort.ContainsEdge(edge))
        //                {
        //                    graphShort.AddEdge(edge);
        //                }
        //    }

        //    return graphShort;
        //}
    }
}
