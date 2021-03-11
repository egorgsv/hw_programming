using System;
using System.Collections.Generic;
using MatrixMult;
using NUnit.Framework;
using QuickGraph;
using QuickGraph.Algorithms;

namespace Tests
{
    [TestFixture]
    class GraphTests
    {
        BidirectionalMatrixGraph<Edge<int>> graph;
        Func<Edge<int>, double> edgeWeights;
        int vertexCount;

        public GraphTests()
        {
            Matrix matrix = new Matrix(
                new int[][] {
                    new int[] {-1, 10, -1, 40, 10},
                    new int[] {-1, -1, -1, -1, -1},
                    new int[] {-1, -1, -1, -1, 5},
                    new int[] {-1, 50, 20, -1, -1},
                    new int[] {20, -1, 10, 70, -1}
                });
            (this.graph, this.edgeWeights, this.vertexCount) = new Graph.Graph(matrix).GetResult();
        }

        [Test]
        public void TestGraphWithOneVertex()
        {
            Matrix matrix = new Matrix(
                new int[][] {
                    new int[] {-1}
                });
            TryFunc<int, IEnumerable<Edge<int>>> tryGetPath = this.createTryGetPath(matrix);
            IEnumerable<Edge<int>> edges;
            Assert.IsFalse(tryGetPath(0, out edges));
        }

        [Test]
        public void TestGraphWithoutEdges()
        {
            Matrix matrix = new Matrix(
                new int[][] {
                    new int[] {-1, -1, -1, -1, -1},
                    new int[] {-1, -1, -1, -1, -1},
                    new int[] {-1, -1, -1, -1, -1},
                    new int[] {-1, -1, -1, -1, -1},
                    new int[] {-1, -1, -1, -1, -1}
                });
            TryFunc<int, IEnumerable<Edge<int>>> tryGetPath = this.createTryGetPath(matrix);
            IEnumerable<Edge<int>> edges;
            for (int i = 0; i < 5; ++i)
                Assert.IsFalse(tryGetPath(i, out edges));
        }

        private TryFunc<int, IEnumerable<Edge<int>>> createTryGetPath(Matrix matrix)
        {
            (BidirectionalMatrixGraph<Edge<int>> graph,
                    Func<Edge<int>, double> edgeWeights, int vertexCount) = new Graph.Graph(matrix).GetResult();
            return graph.ShortestPathsDijkstra(edgeWeights, 0);
        }

        
        [Test]
        public void TestInvalidGraphCreation() {
            int[][] invalid = new int[][] {
                new int[] {1, 2, 3},
                new int[] {4, 5, 6}
            };
            Assert.Throws<ArgumentException>(
                delegate {
                    new Graph.Graph(new Matrix(invalid));
                });
        }

        [Test]
        public void TestCorrectGraphCreation() {
            Edge<int> edge0 = new Edge<int>(0, 3), 
                edge1 = new Edge<int>(2, 4),
                edge2 = new Edge<int>(4, 1);
            Assert.IsTrue(this.graphContainsEdge(this.graph, edge0) && this.edgeWeights(edge0) == 40);
            Assert.IsTrue(this.graphContainsEdge(this.graph, edge1) && this.edgeWeights(edge1) == 5);
            Assert.IsFalse(this.graphContainsEdge(this.graph, edge2));
            for (int i = 0; i < 5; ++i)
                Assert.IsTrue(this.graph.ContainsVertex(i));
        }

        private bool graphContainsEdge(IVertexAndEdgeListGraph<int, Edge<int>> graph, Edge<int> edge) {
            foreach (Edge<int> e in graph.Edges)
                if (e.Source == edge.Source && e.Target == edge.Target)
                    return true;
            return false;
        }

        [Test]
        public void TestShortestPaths() {
            TryFunc<int, IEnumerable<Edge<int>>> tryGetPath = this.graph.ShortestPathsDijkstra(this.edgeWeights, 0);
            IEnumerable<Edge<int>> path;
            Assert.IsFalse(tryGetPath(0, out path));
            tryGetPath(2, out path);
            int[] pattern = new int[] {4, 2};
            Assert.IsTrue(this.checkPath(path, pattern));
        }

        private bool checkPath(IEnumerable<Edge<int>> path, IEnumerable<int> pattern) {
            IEnumerator<Edge<int>> edgesEnumerator = path.GetEnumerator();
            IEnumerator<int> patternEnumerator = pattern.GetEnumerator();
            bool edgesHasNext = edgesEnumerator.MoveNext(),
                patternHasNext = patternEnumerator.MoveNext();
            while (edgesHasNext && patternHasNext) {
                if (edgesEnumerator.Current.Target != patternEnumerator.Current)
                    return false;
                edgesHasNext = edgesEnumerator.MoveNext();
                patternHasNext = patternEnumerator.MoveNext();
            }
            return !(edgesHasNext || patternHasNext);
        }
    }
    
}
