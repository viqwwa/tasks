using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace task20_10
{
    internal class Program
    {
        class Graph
        {
            int count;
            List<int>[] graph;
            int[,] bandwidth;

            public Graph(int n)
            {
                count = n;
                graph = new List<int>[n + 1];
                bandwidth = new int[n + 1, n + 1];

                for (int i = 0; i <= n; i++)
                    graph[i] = new List<int>();
            }

            public void AddEdge(int u,  int v, int c)
            {
                graph[u].Add(v);
                bandwidth[u, v] = c;

                if (!graph[v].Contains(u))
                {
                    graph[v].Add(u);
                }
            }

            public int EdmondsKarp(int source, int to)
            {
                int[,] flow = new int[count + 1, count + 1];
                int maxFlow = 0;

                while (true)
                {
                    int[] parent = new int[count + 1];
                    for (int i = 0; i <= count; i++) parent[i] = -1;
                    parent[source] = source;
                    Queue<int> queue = new Queue<int>();
                    queue.Enqueue(source);
                    bool found = false;

                    while (queue.Count > 0 && !found)
                    {
                        int u = queue.Dequeue();

                        foreach (int w in graph[u])
                        {
                            if (parent[w] == -1 && bandwidth[u, w] - flow[u, w] > 0)
                            {
                                parent[w] = u;

                                if (w == to)
                                {
                                    found = true;
                                    break;
                                }

                                queue.Enqueue(w);
                            }
                        }
                    }

                    if (!found) break;

                    int pathFlow = int.MaxValue;
                    int v = to;

                    while (v != source)
                    {
                        int u = parent[v];
                        pathFlow = Math.Min(pathFlow, bandwidth[u, v] - flow[u, v]);
                        v = u;
                    }

                    v = to;

                    while (v != source)
                    {
                        int u = parent[v];
                        flow[u, v] += pathFlow;
                        flow[v, u] -= pathFlow;
                        v = u;
                    }

                    maxFlow += pathFlow;
                }

                return maxFlow;
            }
        }

        static void Main(string[] args)
        {
            int n = 6;
            Graph g = new Graph(n);

            g.AddEdge(1, 2, 16);
            g.AddEdge(1, 3, 13);

            g.AddEdge(2, 3, 10);
            g.AddEdge(3, 2, 4);
            g.AddEdge(2, 4, 12);
            g.AddEdge(4, 3, 9);
            g.AddEdge(3, 5, 14);
            g.AddEdge(5, 4, 7);

            g.AddEdge(4, 6, 20);
            g.AddEdge(5, 6, 4);

            int maxFlow = g.EdmondsKarp(1, n);
            Console.WriteLine(maxFlow);
        }
    }
}
