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
        public Func<Edge<int>, double> edgeWeights;
        int[][] adjacencyMatrix;
        public int vertexCount;

        public Graph(Matrix matrix)
        {
            if (matrix.m != matrix.n)
                throw new ArgumentException("This is not a square matrix!");
            adjacencyMatrix = matrix.GetContent();
            graph = null;
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
    }
}
