using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace task20_16
{
    internal class Program
    {
        class Graph
        {
            int count;
            List<int>[] graph;
            int time;

            public Graph(int n)
            {
                count = n;
                graph = new List<int>[n + 1];
                time = 0;

                for (int i = 0; i <= n; i++)
                    graph[i] = new List<int>();
            }

            public void AddEdge(int u, int v)
            {
                graph[u].Add(v);
                graph[v].Add(u);
            }

            public List<int> FindArtPoints()
            {
                int[] visited = new int[count + 1];
                int[] tin = new int[count + 1];
                int[] low = new int[count + 1];
                int[] parent = new int[count + 1];
                List<int> artPoints = new List<int>();

                for (int i = 0; i <= count; i++)
                {
                    tin[i] = -1;
                    low[i] = -1;
                    parent[i] = -1;
                }

                void DFS(int u, int p)
                {
                    visited[u] = 1;
                    tin[u] = time;
                    low[u] = time;
                    time++;
                    int children = 0;
                    bool isArt = false;

                    foreach (int v in graph[u])
                    {
                        if (v == p) continue;

                        if (visited[v] == 0)
                        {
                            parent[v] = u;
                            children++;
                            DFS(v, u);
                            low[u] = Math.Min(low[u], low[v]);

                            if (p != -1 && low[v] >= tin[u])
                                isArt = true;
                        }
                        else
                            low[u] = Math.Min(low[u], tin[v]);
                    }

                    if (p == -1 && children > 1) isArt = true;
                    if (isArt) artPoints.Add(u);
                }

                for (int i = 1; i <= count; i++)
                {
                    if (visited[i] == 0)
                    {
                        time = 0;
                        DFS(i, -1);
                    }
                }

                return artPoints;
            }
        }

        static void Main(string[] args)
        {
            Graph g = new Graph(7);

            g.AddEdge(1, 2);
            g.AddEdge(2, 3);
            g.AddEdge(3, 1);
            g.AddEdge(3, 4);
            g.AddEdge(4, 5);
            g.AddEdge(5, 6);
            g.AddEdge(6, 7);
            g.AddEdge(7, 5);

            List<int> points = g.FindArtPoints();

            if (points.Count == 0)
            {
                Console.WriteLine("Шарниров не обнаружено.");
            }
            else
            {
                foreach (int p in points)
                {
                    Console.WriteLine(p);
                }
            }
        }
    }
}
