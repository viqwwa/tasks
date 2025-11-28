using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace task_7
{
    internal class Program
    {
        public class Bid
        {
            public int priority;
            public int id;
            public int iteration;

            public Bid()
            {
                priority = 0;
                id = 0;
                iteration = 0;
            }

            public Bid(int priority, int id, int iteration)
            {
                this.priority = priority;
                this.id = id;
                this.iteration = iteration;
            }

            public void Log(string operation)
            {
                try
                {
                    using (StreamWriter sr = new StreamWriter("log.txt", true))
                    {
                        sr.WriteLine($"{operation} {id} {priority} {iteration}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            public void Print()
            {
                Console.WriteLine($"{id} {priority} {iteration}");
            }
        }

        public static IComparer<Bid> BidComparer =>
            Comparer<Bid>.Create((x, y) =>
            {
                if (x == null && y == null) return 0;
                if (x == null) return -1;
                if (y == null) return 1;
                int priorityCompare = x.priority.CompareTo(y.priority);
                return priorityCompare != 0 ? priorityCompare : x.id.CompareTo(y.id);
            });

        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());
            MyPriorityQueue<Bid> queue = new MyPriorityQueue<Bid>(n, BidComparer);
            Random random = new Random();
            int index = 1;
            Bid lastDeleted = new Bid();

            for (int i = 0; i < n; i++)
            {
                int count = random.Next(1, 11);

                for (int j = 0; j < count; j++)
                {
                    int priority = random.Next(1, 6);
                    Bid bid = new Bid(priority, index++, i);
                    queue.Add(bid);
                    bid.Log("ADD");
                }

                lastDeleted = queue.Poll();
                lastDeleted.Log("REMOVE");
            }

            while (!queue.IsEmpty())
            {
                lastDeleted = queue.Poll();
                lastDeleted.Log("REMOVE");
            }

            Console.WriteLine("Заявка, которая ожидала максимальное время");
            lastDeleted.Print();
        }
    }
}
