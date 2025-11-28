using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_7
{
    public enum HeapType
    {
        MinHeap, MaxHeap
    }
    public class Heap<T>
    {
        public T[] array { get; set; }
        public int size { get; set; }
        private int capacity;
        private HeapType type;
        private IComparer<T> comparer;

        public Heap(T[] array, HeapType heapType, IComparer<T> comparer)
        {
            size = 0;
            capacity = array.Length;
            this.array = new T[capacity];
            type = heapType;
            this.comparer = comparer ?? Comparer<T>.Default;
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
                if (capacity < 64)
                {
                    capacity += 2;
                }
                else
                {
                    capacity += capacity / 2;
                }
            }

            T[] newArray = new T[capacity];
            for (int i = 0; i < size; i++)
            {
                newArray[i] = array[i];
            }
            array = newArray;

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

            Heap<T> newHeap = new Heap<T>(mergedArray, heap1.type, heap1.comparer);
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
            int result = comparer.Compare(element1, element2);
            return type == HeapType.MinHeap ? result < 0 : result > 0;
        }

        public void IsEmpty()
        {
            if (size == 0)
            {
                throw new InvalidOperationException("Куча пуста");
            }
        }
    }
}
