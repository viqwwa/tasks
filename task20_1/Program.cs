using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task20_1
{
    internal class Program
    {
        public class TransitiveClosure
        {
            private int vertices;
            private List<int>[] adj;
            private bool[,] closure;

            public TransitiveClosure(int v)
            {
                vertices = v;
                adj = new List<int>[v + 1];
                closure = new bool[v + 1, v + 1];

                for (int i = 1; i <= v; i++)
                {
                    adj[i] = new List<int>();
                }
            }

            public void AddEdge(int u, int v)
            {
                adj[u].Add(v);
            }

            private void DFS(int source, int current)
            {
                closure[source, current] = true;

                foreach (int neighbor in adj[current])
                {
                    if (!closure[source, neighbor])
                    {
                        DFS(source, neighbor);
                    }
                }
            }

            public void BuildClosure()
            {
                for (int i = 1; i <= vertices; i++)
                {
                    DFS(i, i);
                }
            }

            public void PrintClosure()
            {
                Console.WriteLine("Матрица транзитивного замыкания:");
                for (int i = 1; i <= vertices; i++)
                {
                    for (int j = 1; j <= vertices; j++)
                    {
                        Console.Write((closure[i, j] ? 1 : 0) + " ");
                    }
                    Console.WriteLine();
                }
            }
        }
        static void Main(string[] args)
        {
            TransitiveClosure graph = new TransitiveClosure(4);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);
            graph.AddEdge(3, 1);
            graph.AddEdge(3, 4);

            graph.BuildClosure();
            graph.PrintClosure();
        }
    }
}
