using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace task_6
{
    internal class Program
    {
        class MyPriorityQueue<T>
        {
            private Heap<T> queue;
            private int size;
            private IComparer<T> comparer;

            public MyPriorityQueue()
            {
                T[] array = new T[11];
                size = 0;
                comparer = Comparer<T>.Default;
                queue = new Heap<T>(array, HeapType.MinHeap, comparer);
            }

            public MyPriorityQueue(T[] array)
            {
                size = array.Length;
                comparer = Comparer<T>.Default;
                queue = new Heap<T>(array, HeapType.MinHeap, comparer);
            }

            public MyPriorityQueue(int initialCapacity)
            {
                if (initialCapacity < 0)
                {
                    throw new ArgumentException("Ёмкость должна быть положительной");
                }

                T[] array = new T[initialCapacity];
                size = 0;
                comparer = Comparer<T>.Default;
                queue = new Heap<T>(array, HeapType.MinHeap, comparer);
            }

            public MyPriorityQueue(int initialCapacity, IComparer<T> comparer)
            {
                if (initialCapacity < 0)
                {
                    throw new ArgumentException("Ёмкость должна быть положительной");
                }

                size = 0;
                this.comparer = comparer;
                T[] array = new T[initialCapacity];
                queue = new Heap<T>(array, HeapType.MinHeap, comparer);
            }

            public MyPriorityQueue(MyPriorityQueue<T> q)
            {
                size = q.size;
                comparer = q.comparer;
                T[] newArray = new T[q.queue.array.Length];
                Array.Copy(q.queue.array, newArray, q.size);
                queue = new Heap<T>(newArray, HeapType.MinHeap, comparer);
            }

            public void Add(T item)
            {
                queue.Insert(item);
                size++;
            }

            public void AddAll(T[] array)
            {
                foreach (T item in array)
                {
                    queue.Insert(item);
                    size++;
                }
            }

            public void Clear()
            {
                size = 0;
                queue.size = 0;
            }

            public bool Contains(T itemSearch)
            {
                for (int i = 0; i < size; i++)
                {
                    if (comparer.Compare(itemSearch, queue.array[i]) == 0) return true;
                }
                return false;
            }

            public bool ContainsAll(T[] array)
            {
                foreach (T item in array)
                {
                    if (!Contains(item)) return false;
                }
                return true;
            }

            public bool IsEmpty()
            {
                return size == 0;
            }

            public void Remove(T itemRemove)
            {
                if (IsEmpty())
                {
                    throw new InvalidOperationException("Очередь пуста");
                }
                for (int i = 0; i < size; i++)
                {
                    if (comparer.Compare(itemRemove, queue.array[i]) == 0)
                    {
                        T removedItem = queue.array[i];
                        queue.array[i] = queue.array[size - 1];
                        size--;
                        queue.size--;
                        queue.SiftDown(i);
                        queue.SiftUp(i);
                        break;
                    }
                }
            }

            public void RemoveAll(T[] array)
            {
                foreach (T item in array)
                {
                    Remove(item);
                }
            }

            public void RetainAll(T[] array)
            {
                bool flag = false;
                foreach (T item in queue.array)
                {
                    foreach (T itemRetain in array)
                    {
                        if (comparer.Compare(itemRetain, item) == 0) flag = true;
                    }
                    if (!flag)
                    {
                        Remove(item);
                    }
                }
            }

            public int Size() => size;

            public T[] ToArray()
            {
                return queue.array;
            }

            public T[] ToArray(T[] array)
            {
                if (array == null)
                {
                    return ToArray();
                }
                if (array.Length < size)
                {
                    throw new ArgumentException("Длина массива недостаточна");
                }
                for (int i = 0; i < size; i++)
                {
                    array[i] = queue.array[i];
                }
                return array;
            }

            public T Element()
            {
                return queue.Peek();
            }

            public Object Peek()
            {
                try
                {
                    return queue.Peek();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            public Object Poll()
            {
                try
                {
                    size--;
                    return queue.Pop();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            public bool Offer(T item)
            {
                if (!typeof(T).IsValueType && item == null)
                {
                    return false;
                }
                Add(item);
                return true;
            }

            public void Print()
            {
                for (int i = 0; i < size; i++)
                {
                    Console.Write(queue.array[i]);
                }
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            try
            {
                MyPriorityQueue<int> queue = new MyPriorityQueue<int>();
                queue.Remove(2);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
