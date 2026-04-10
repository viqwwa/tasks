using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task19
{
    internal class Program
    {
        class ReverseComparator<T> : IComparer<T>
        {
            private IComparer<T> original;
            public ReverseComparator(IComparer<T> original) => this.original = original;

            public int Compare(T x, T y) => original.Compare(y, x);
        }

        class MyTreeSet<E> where E : IComparable<E>
        {
            MyTreeMap<E, object> m;
            static object PRESENT = new object();
            private E lowerBound;
            private E upperBound;
            private bool lowerActive, upperActive;
            private bool lowerInclusive, upperInclusive;

            private MyTreeSet(MyTreeMap<E, object> map,
                      E low, bool lowAct, bool lowIncl,
                      E high, bool highAct, bool highIncl)
            {
                m = map;
                lowerBound = low;
                lowerActive = lowAct;
                lowerInclusive = lowIncl;
                upperBound = high;
                upperActive = highAct;
                upperInclusive = highIncl;
            }

            public MyTreeSet() : this(new MyTreeMap<E, object>(),
                              default, false, false, default, false, false)
            { }

            public MyTreeSet(IComparer<E> comparator) : this(new MyTreeMap<E, object>(comparator),
                                                            default, false, false, default, false, false)
            { }

            public MyTreeSet(MyTreeMap<E, object> m) : this(m, default, false, false, default, false, false)
            { }

            public MyTreeSet(E[] a) : this()
            {
                if (a == null) throw new ArgumentNullException("Был передан null");
                foreach (E item in a) Add(item);
            }

            public MyTreeSet(SortedSet<E> s) : this()
            {
                foreach (E item in s)
                {
                    m.Put(item, PRESENT);
                }
            }

            public void Add(E e)
            {
                if (!IsInBounds(e))
                    throw new ArgumentException("Элемент выходит за установленные границы этого подмножества.");

                m.Put(e, PRESENT);
            }

            public void AddAll(E[] a)
            {
                foreach (E item in a)
                {
                    Add(item);
                }
            }

            public void Clear()
            {
                m.Clear();
            }

            public bool Contains(object o)
            {
                if (o == null) return false;

                if (o is E genericKey)
                {
                    if (IsInBounds(genericKey) && m.ContainsKey(genericKey)) return true;
                    return false;
                }

                return false;
            }

            public bool ContainsAll(E[] a)
            {
                foreach (E item in a)
                {
                    if (!Contains(item)) return false;
                }

                return true;
            }

            public bool IsEmpty()
            {
                return m.IsEmpty();
            }

            public void Remove(object o)
            {
                if (o == null) throw new ArgumentNullException("Ключ не может быть null");

                if (o is E genericKey)
                {
                    if (IsInBounds(genericKey))
                        m.Remove(genericKey);
                }
            }

            public void RemoveAll(E[] a)
            {
                foreach (E item in a)
                {
                    Remove(item);
                }
            }

            public void RetainAll(E[] a)
            {
                MyTreeSet<E> tempSet = new MyTreeSet<E>(m.Comparator);
                foreach (E x in a)
                {
                    if (IsInBounds(x))
                        tempSet.Add(x);
                }
                var allCurrent = m.KeyList(lowerBound, lowerActive, lowerInclusive, upperBound, upperActive, upperInclusive);

                foreach (E item in allCurrent)
                {
                    if (!tempSet.Contains(item))
                    {
                        Remove(item);
                    }
                }
            }

            public int Size()
            {
                return m.CountInRange(m.Root, lowerBound, lowerActive, lowerInclusive, upperBound, upperActive, upperInclusive);
            }

            public E[] ToArray()
            {
                List<E> listInRange = m.KeyList(lowerBound, lowerActive, lowerInclusive, upperBound, upperActive, upperInclusive);
                return listInRange.ToArray();
            }

            public E First()
            {
                return m.FirstKey(lowerBound, lowerActive, lowerInclusive, upperBound, lowerActive, upperInclusive);
            }

            public E Last()
            {
                return m.LastKey(lowerBound, lowerActive, lowerInclusive, upperBound, lowerActive, upperInclusive);
            }

            public MyTreeSet<E> SubSet(E fromElement,  E toElement)
            {
                if (m.Comparator.Compare(fromElement, toElement) > 0)
                    throw new ArgumentException("Начальный ключ не может быть больше конечного");

                if (!IsInBounds(fromElement) || !IsInBounds(toElement))
                {
                    throw new ArgumentException("Границы выходят за пределы текущего подмножества.");
                }

                MyTreeMap<E, object> subMap = m.SubMap(fromElement, toElement);
                return new MyTreeSet<E>(subMap);
            }

            public MyTreeSet<E> HeadSet(E toElement)
            {
                if (!IsInBounds(toElement))
                {
                    throw new ArgumentException("Граница выходит за пределы текущего подмножества.");
                }

                MyTreeMap<E, object> headMap = m.HeadMap(toElement);
                return new MyTreeSet<E>(headMap);
            }

            public MyTreeSet<E> TailSet(E fromElement)
            {
                if (!IsInBounds(fromElement))
                {
                    throw new ArgumentException("Граница выходит за пределы текущего подмножества.");
                }

                MyTreeMap<E, object> tailMap = m.TailMap(fromElement);
                return new MyTreeSet<E>(tailMap);
            }

            public object Ceiling(E obj)
            {
                object result = m.CeilingKey(obj);

                if (result is E genericKey && IsInBounds(genericKey))
                {
                    return result;
                }

                return null;
            }

            public object Floor(E obj)
            {
                object result = m.FloorKey(obj);

                if (result is E genericKey && IsInBounds(genericKey))
                {
                    return result;
                }

                return null;
            }

            public object Higher(E obj)
            {
                object result = m.HigherKey(obj);

                if (result is E genericKey && IsInBounds(genericKey))
                {
                    return result;
                }

                return null;
            }

            public object Lower(E obj)
            {
                object result = m.LowerKey(obj);

                if (result is E genericKey && IsInBounds(genericKey))
                {
                    return result;
                }

                return null;
            }

            public MyTreeSet<E> HeadSet(E upperBound, bool incl)
            {
                return new MyTreeSet<E>(m, lowerBound, lowerActive, lowerInclusive, upperBound, true, incl);
            }

            public MyTreeSet<E> SubSet(E lowerBound, bool lowIncl, E upperBound, bool highIncl)
            {
                return new MyTreeSet<E>(m, lowerBound, true, lowIncl, upperBound, true, highIncl);
            }

            public MyTreeSet<E> TailSet(E fromElement, bool inclusive)
            {
                return new MyTreeSet<E>(m, fromElement, true, inclusive, upperBound, upperActive, upperInclusive);
            }

            public object PollLast()
            {
                return m.PollLastEntry(lowerBound, lowerActive, lowerInclusive, upperBound, upperActive, upperInclusive);
            }

            public object PollFirst()
            {
                return m.PollFirstEntry(lowerBound, lowerActive, lowerInclusive, upperBound, upperActive, upperInclusive);
            }

            public IEnumerator<E> DescendingIterator()
            {
                List<E> listInRange = new List<E>();
                m.FillListInRange(m.Root, listInRange, lowerBound, lowerActive, lowerInclusive, upperBound, upperActive, upperInclusive);

                listInRange.Reverse();

                return listInRange.GetEnumerator();
            }

            public MyTreeSet<E> DescendingSet()
            {
                var revComp = new ReverseComparator<E>(m.Comparator);
                MyTreeMap<E, object> revMap = new MyTreeMap<E, object>(revComp, m.Root, Size());
                return new MyTreeSet<E>(revMap, upperBound, upperActive, upperInclusive,
                                        lowerBound, lowerActive, lowerInclusive);
            }

            private bool IsInBounds(E item)
            {
                if (upperActive)
                {
                    int cmp = m.Comparator.Compare(item, upperBound);
                    if (upperInclusive ? cmp > 0 : cmp >= 0) return false;
                }

                if (lowerActive)
                {
                    int cmp = m.Comparator.Compare(item, lowerBound);
                    if (lowerInclusive ? cmp < 0 : cmp <= 0) return false;
                }

                return true;
            }

        }

        static void Main(string[] args)
        {
        }
    }
}
