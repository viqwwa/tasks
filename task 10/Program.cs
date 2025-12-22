using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace task_10
{
    internal class Program
    {
        class MyVector<T>
        {
            T[] elementData;
            int elementCount;
            int capacityIncrement;

            public MyVector(int initialCapacity, int capacityIncrement)
            {
                elementData = new T[initialCapacity];
                this.capacityIncrement = capacityIncrement;
                elementCount = 0;
            }

            public MyVector(int initialCapacity)
            {
                elementData = new T[initialCapacity];
                capacityIncrement = 0;
                elementCount = 0;
            }

            public MyVector()
            {
                elementData = new T[10];
                capacityIncrement = 0;
                elementCount = 0;
            }

            public MyVector(T[] array)
            {
                elementCount = array.Length;
            }

            public void Add(T element)
            {
                if (elementCount == elementData.Length)
                {
                    IncreaseCapacity();
                }

                elementData[elementCount++] = element;
            }

            public void addAll(T[] array)
            {
                foreach (T element in array)
                {
                    Add(element);
                }
            }

            public void Clear()
            {
                elementCount = 0;
            }

            public bool Contains(T element)
            {
                foreach (T el in elementData)
                {
                    if (Comparer<T>.Default.Compare(el, element) == 0)
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
                    if (!Contains(element))
                    {
                        return false;
                    }
                }
                return true;
            }

            public bool IsEmpty()
            {
                return elementCount == 0;
            }

            public void Remove(T element)
            {
                for (int i = 0; i < elementCount; i++)
                {
                    if (Comparer<T>.Default.Compare(elementData[i], element) == 0)
                    {
                        for (int j = i; j < elementCount - 1; j++)
                        {
                            elementData[j] = elementData[j + 1];
                        }
                        i--;
                        elementCount--;
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
                MyVector<T> arrayList = new MyVector<T>(array);
                for (int i = 0; i < elementCount; i++)
                {
                    if (!arrayList.Contains(elementData[i]))
                    {
                        Remove(elementData[i]);
                    }
                }
            }

            public int Size() => elementCount;

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
                if (array.Length < elementCount)
                {
                    throw new ArgumentException("Длина массива недостаточна");
                }
                for (int i = 0; i < elementCount; i++)
                {
                    array[i] = elementData[i];
                }
                return array;
            }

            public void Add(int index, T element)
            {
                if (index >= elementCount)
                {
                    throw new IndexOutOfRangeException("Такого индекса нет в массиве");
                }

                if (elementCount == elementData.Length)
                {
                    IncreaseCapacity();
                }

                for (int i = elementCount + 1; i > index; i--)
                {
                    elementData[i] = elementData[i - 1];
                }

                elementData[index] = element;
            }

            public void AddAll(int index, T[] array)
            {
                if (index >= elementCount)
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
                if (index >= elementCount)
                {
                    throw new IndexOutOfRangeException("Такого индекса нет в массиве");
                }

                return elementData[index];
            }

            public int IndexOf(T element)
            {
                for (int i = 0; i < elementCount; i++)
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

                for (int i = 0; i < elementCount; i++)
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
                if (index >= elementCount)
                {
                    throw new IndexOutOfRangeException("Такого индекса нет в массиве");
                }

                T element = elementData[index];
                Remove(element);
                return element;
            }

            public void Set(int index, T element)
            {
                if (index >= elementCount)
                {
                    throw new IndexOutOfRangeException("Такого индекса нет в массиве");
                }

                elementData[index] = element;
            }

            public T[] SubList(int fromIndex, int toIndex)
            {
                if (fromIndex >= elementCount || toIndex >= elementCount || toIndex < fromIndex)
                {
                    throw new IndexOutOfRangeException("Некорректные индексы");
                }

                T[] subList = new T[toIndex - fromIndex];

                Array.Copy(elementData, fromIndex, subList, 0, subList.Length);

                return subList;
            }

            public T FirstElement()
            {
                return elementData[0];
            }

            public T LastElement()
            {
                return elementData[elementCount - 1];
            }

            public void RemoveElementAt(int pos)
            {
                if (pos >= elementCount)
                {
                    throw new IndexOutOfRangeException("Такого индекса нет в массиве");
                }

                T element = elementData[pos];
                Remove(element);
            }

            public void RemoveRange(int begin, int end)
            {
                if (begin >= elementCount || end >= elementCount || begin < end)
                {
                    throw new IndexOutOfRangeException("Некорректные индексы");
                }

                for (int i = begin; i < end; i++)
                {
                    RemoveElementAt(i);
                }
            }

            public void IncreaseCapacity()
            {
                T[] newElementData;

                if (capacityIncrement == 0)
                {
                    newElementData = new T[elementCount * 2];
                }
                else
                {
                    newElementData = new T[elementCount + capacityIncrement];
                }

                elementData = newElementData;
            }
        }

        static void Main(string[] args)
        {
        }
    }
}
