using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_9
{
    public class MyArrayList<T>
    {
        private T[] elementData;
        private int size;

        public MyArrayList()
        {
            elementData = new T[0];
            size = 0;
        }

        public MyArrayList(T[] array)
        {
            size = array.Length;
            elementData = new T[size];
            Array.Copy(array, elementData, size);
        }

        public MyArrayList(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentException("Ёмкость должна быть положительной");
            }
            size = 0;
            elementData = new T[capacity];
        }

        public void Add(T element)
        {
            if (size == elementData.Length)
            {
                IncreaseCapacity();
            }

            elementData[size++] = element;
        }

        public void AddAll(T[] array)
        {
            foreach (T element in array)
            {
                Add(element);
            }
        }

        public void Clear()
        {
            size = 0;
        }

        public bool Contains(T element)
        {
            for (int i = 0; i < size; i++)
            {
                if (Comparer<T>.Default.Compare(elementData[i], element) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsAll(T[] array)
        {
            foreach (T element in array)
            {
                if (!Contains(element)) return false;
            }
            return true;
        }

        public bool IsEmpty()
        {
            return size == 0;
        }

        public void Remove(T element)
        {
            for (int i = 0; i < size; i++)
            {
                if (Comparer<T>.Default.Compare(elementData[i], element) == 0)
                {
                    for (int j = i; j < size - 1; j++)
                    {
                        elementData[j] = elementData[j + 1];
                    }
                    i--;
                    size--;
                }
            }
        }

        public void RemoveAll(T[] array)
        {
            foreach (T element in array)
            {
                Remove(element);
            }
        }

        public void RetainAll(T[] array)
        {
            MyArrayList<T> arrayList = new MyArrayList<T>(array);
            for (int i = 0; i < size; i++)
            {
                if (!arrayList.Contains(elementData[i]))
                {
                    Remove(elementData[i]);
                }
            }
        }

        public int Size() => size;

        public T[] ToArray()
        {
            return elementData;
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
                array[i] = elementData[i];
            }
            return array;
        }

        public void Add(int index, T element)
        {
            if (index >= size)
            {
                throw new IndexOutOfRangeException("Такого индекса нет в массиве");
            }

            if (size == elementData.Length)
            {
                IncreaseCapacity();
            }

            for (int i = size + 1; i > index; i--)
            {
                elementData[i] = elementData[i - 1];
            }

            elementData[index] = element;
        }

        public void AddAll(int index, T[] array)
        {
            if (index >= size)
            {
                throw new IndexOutOfRangeException("Такого индекса нет в массиве");
            }

            foreach (T element in array)
            {
                Add(index++, element);
            }
        }

        public T Get(int index)
        {
            if (index >= size)
            {
                throw new IndexOutOfRangeException("Такого индекса нет в массиве");
            }

            return elementData[index];
        }

        public int IndexOf(T element)
        {
            for (int i = 0; i < size; i++)
            {
                if (Comparer<T>.Default.Compare(elementData[i], element) == 0)
                {
                    return i;
                }
            }
            return -1;
        }

        public int LastIndexOf(T element)
        {
            int lastIndex = -1;

            for (int i = 0; i < size; i++)
            {
                if (Comparer<T>.Default.Compare(elementData[i], element) == 0)
                {
                    lastIndex = i;
                }
            }

            return lastIndex;
        }

        public T Remove(int index)
        {
            if (index >= size)
            {
                throw new IndexOutOfRangeException("Такого индекса нет в массиве");
            }

            T element = elementData[index];
            Remove(element);
            return element;
        }

        public void Set(int index, T element)
        {
            if (index >= size)
            {
                throw new IndexOutOfRangeException("Такого индекса нет в массиве");
            }

            elementData[index] = element;
        }

        public T[] SubList(int fromIndex, int toIndex)
        {
            if (fromIndex >= size || toIndex >= size || toIndex < fromIndex)
            {
                throw new IndexOutOfRangeException("Некорректные индексы");
            }

            T[] subList = new T[toIndex - fromIndex];

            Array.Copy(elementData, fromIndex, subList, 0, subList.Length);

            return subList;
        }

        public void IncreaseCapacity()
        {
            T[] newElementData = new T[size + 3 * size / 2 + 1];
            Array.Copy(elementData, newElementData, size);
            elementData = newElementData;
        }
    }
}
