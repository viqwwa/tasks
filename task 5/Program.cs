using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace task_5
{
    internal class Program
    {
        enum HeapType
        {
            MinHeap, MaxHeap
        }

        class Heap<T> where T : IComparable<T>
        {
            private T[] array;
            private int size;
            private int capacity;
            private HeapType type;

            public Heap(T[] array, HeapType heapType)
            {
                size = 0;
                capacity = array.Length;
                this.array = new T[capacity];
                type = heapType;

                for (int i = 0; i < capacity; i++)
                {
                    this.array[i] = array[i];
                    size++;
                    SiftUp(i);
                }
            }

            public T Peek()
            {
                IsEmpty();
                return array[0];
            }

            public void Insert(T element)
            {
                if (size == capacity)
                {
                    capacity *= 2;
                    T[] newArray = new T[capacity];
                    for (int i = 0; i < size; i++)
                    {
                        newArray[i] = array[i];
                    }
                    array = newArray;
                }

                array[size++] = element;
                SiftUp(size - 1);
            }

            public T Pop()
            {
                IsEmpty();
                T top = array[0];
                T lastValue = array[size - 1];
                array[0] = lastValue;
                size--;
                SiftDown(0);
                return top;
            }

            public void ChangeKey(int index, T newValue)
            {
                if (index > size - 1)
                {
                    throw new ArgumentOutOfRangeException("Такого элемента нет в куче");
                }

                T oldValue = array[index];
                array[index] = newValue;

                if (Compare(oldValue, newValue))
                {
                    SiftDown(index);
                }
                else
                {
                    SiftUp(index);
                }
            }

            static public Heap<T> MergeHeaps(Heap<T> heap1, Heap<T> heap2)
            {
                if (heap1.type != heap2.type)
                {
                    throw new InvalidOperationException("Нельзя произвести слияние куч разного типа");
                }

                T[] mergedArray = new T[heap1.size + heap2.size];

                for (int i = 0; i < heap1.size; i++)
                {
                    mergedArray[i] = heap1.array[i];
                }

                for (int i = 0; i < heap2.size; i++)
                {
                    mergedArray[i + heap1.size] = heap2.array[i];
                }

                Heap<T> newHeap = new Heap<T>(mergedArray, heap1.type);
                return newHeap;
            }

            public void SiftUp(int index)
            {
                while (index > 0)
                {
                    int parentIndex = (index - 1) / 2;

                    if (Compare(array[index], array[parentIndex]))
                    {
                        (array[index], array[parentIndex]) = (array[parentIndex], array[index]);
                        index = parentIndex;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            public void SiftDown(int index)
            {
                while (index * 2 + 1 < size)
                {
                    int leftIndex = index * 2 + 1;
                    int rightIndex = index * 2 + 2;
                    int minChildIndex = leftIndex;

                    if (rightIndex < size && Compare(array[rightIndex], array[leftIndex]))
                    {
                        minChildIndex = rightIndex;
                    }

                    if (Compare(array[index], array[minChildIndex]))
                    {
                        break;
                    }
                    else
                    {
                        (array[index], array[minChildIndex]) = (array[minChildIndex], array[index]);
                        index = minChildIndex;
                    }
                }
            }

            private bool Compare(T element1, T element2)
            {
                int result = element1.CompareTo(element2);
                return type == HeapType.MinHeap ? result < 0 : result > 0;
            }

            public void IsEmpty()
            {
                if (size == 0)
                {
                    throw new InvalidOperationException("Куча пуста");
                }
            }

            public void Print()
            {
                for (int i = 0; i < size; i++)
                {
                    Console.Write(array[i] + " ");
                }
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            int[] array1 = { 1, 4, 3, 8, 2, 5 }, array3 = { 3, 1, 4, 5, 9, 2 };
            string[] array2 = { "hi", "vika", "program", "helpmepls", "abc", "c#" };

            Heap<int> heap1 = new Heap<int>(array1, HeapType.MinHeap);
            Heap<string> heap2 = new Heap<string>(array2, HeapType.MaxHeap);
            Heap<int> heap3 = new Heap<int>(array3, HeapType.MinHeap);

            try
            {
                heap3.Print();
                Console.WriteLine(heap1.Pop());
                Console.WriteLine(heap2.Pop());
                Console.WriteLine(heap3.Pop());
                Console.WriteLine();

                Console.WriteLine(heap1.Peek());
                Console.WriteLine(heap2.Peek());
                Console.WriteLine(heap3.Peek());
                Console.WriteLine();

                heap1.ChangeKey(2, 1);
                heap2.ChangeKey(3, "you");
                heap3.ChangeKey(4, 6);

                heap1.Print();
                heap2.Print();
                heap3.Print();

                Heap<int> heap4 = Heap<int>.MergeHeaps(heap1, heap3);
                heap4.Print();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
