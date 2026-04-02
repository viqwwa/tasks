using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task1_sem4
{
    internal class Program
    {
        class Node<K, V>
        {
            public K Key {  get; set; }
            public V Value { get; set; }
            public Node<K, V> Left { get; set; }
            public Node<K, V> Right { get; set; }

            public Node(K key, V value)
            {
                Key = key;
                Value = value;
            }
        }

        class MyTreeMap<K, V>
        {
            private IComparer<K> comparator;
            private Node<K, V> root;
            private int size;

            public MyTreeMap()
            {
                comparator = Comparer<K>.Default;
                root = null;
                size = 0;
            }

            public MyTreeMap(IComparer<K> comparator)
            {
                this.comparator = comparator;
                root = null;
                size = 0;
            }

            public void Clear()
            {
                root = null;
                size = 0;
            }

            public bool ContainsKey(Object key)
            {
                if (key == null) return false;

                if (key is K genericKey)
                {
                    if (SearchKey(genericKey) != null) return true;
                    return false;
                }

                return false;
            }

            private Node<K, V> SearchKey(K key)
            {
                Node<K, V> node = root;

                while (node != null)
                {
                    int compareResult = comparator.Compare(key, node.Key);

                    if (compareResult == 0)
                        return node;
                    else if (compareResult < 0)
                        node = node.Left;
                    else
                        node = node.Right;
                }

                return null;
            }

            public bool ContainsValue(object value)
            {
                if (value == null) return false;

                if (value is V genericValue)
                {
                    return SearchValue(genericValue, root);
                }
                return false;
            }

            private bool SearchValue(V value, Node<K, V> node)
            {
                if (node == null) return false;
                if (value.Equals(node.Value)) return true;
                return SearchValue(value, node.Left) || SearchValue(value, node.Right);
            }

            public ISet<KeyValuePair<K, V>> EntrySet()
            {
                var set = new HashSet<KeyValuePair<K, V>>();
                FillEntrySet(root, set);
                return set;
            }

            private void FillEntrySet(Node<K, V> node, ISet<KeyValuePair<K, V>> set)
            {
                if (node == null) return;
                FillEntrySet(node.Left, set);
                set.Add(new KeyValuePair<K, V>(node.Key, node.Value));
                FillEntrySet(node.Right, set);
            }

            public Object Get(Object key)
            {
                if (key == null) return null;

                if (key is K genericKey)
                {
                    Node<K, V> node = SearchKey(genericKey);
                    return node != null ? (object)node.Value : null;
                }

                return null;
            }

            public bool IsEmpty()
            {
                return size == 0;
            }

            public ISet<K> KeySet()
            {
                var entries = EntrySet();
                return new HashSet<K>(entries.Select(e => e.Key));
            }

            public void Put(K key, V value)
            {
                if (key == null) throw new ArgumentNullException("Ключ не может быть null");
                root = Put(root, key, value);
            }

            private Node<K, V> Put(Node<K, V> node, K key, V value)
            {
                if (node == null)
                {
                    size++;
                    return new Node<K, V>(key, value);
                }

                int compareResult = comparator.Compare(key, node.Key);

                if (compareResult == 0)
                {
                    node.Value = value;
                    return node;
                }
                else if (compareResult < 0)
                    node.Left = Put(node.Left, key, value);
                else
                    node.Right = Put(node.Right, key, value);

                return node;
            }

            public void Remove(Object key)
            {
                if (key == null) throw new ArgumentNullException("Ключ не может быть null");

                if (key is K genericKey)
                {
                    root = Remove(root, genericKey);
                }
            }

            private Node<K, V> Remove(Node<K, V> node, K key)
            {
                if (node == null) return null;
                int compareResult = comparator.Compare(key, node.Key);

                if (compareResult == 0)
                {
                    size--;
                    if (node.Left == null) return node.Right;
                    if (node.Right == null) return node.Left;

                    Node<K, V> successor = node.Right;

                    while (successor.Left != null)
                    {
                        successor = successor.Left;
                    }

                    node.Key = successor.Key;
                    node.Value = successor.Value;
                    size++;
                    node.Right = Remove(node.Right, successor.Key);
                }
                else if (compareResult < 0)
                    node.Left = Remove(node.Left, key);
                else
                    node.Right = Remove(node.Right, key);

                if (node == null) return null;
                return node;
            }

            public int Size => size;

            public K FirstKey()
            {
                var firstEntry = FirstEntry();
                if (firstEntry == null) throw new InvalidOperationException("Дерево пустое");
                return firstEntry.Value.Key;
            }

            public K LastKey()
            {
                var lastEntry = LastEntry();
                if (lastEntry == null) throw new InvalidOperationException("Дерево пустое");
                return lastEntry.Value.Key;
            }

            public MyTreeMap<K, V> HeadMap(K end)
            {
                MyTreeMap<K, V> head = new MyTreeMap<K, V>(comparator);
                FillHeadMap(head, root, end);
                return head;
            }

            private void FillHeadMap(MyTreeMap<K, V> head, Node<K, V> node, K end)
            {
                if (node == null) return;
                int compareResult = comparator.Compare(node.Key, end);

                FillHeadMap(head, node.Left, end);

                if (compareResult < 0)
                {
                    head.Put(node.Key, node.Value);
                    FillHeadMap(head, node.Right, end);
                }  
            }

            public MyTreeMap<K, V> SubMap(K start, K end)
            {
                if (comparator.Compare(start, end) > 0)
                    throw new ArgumentException("Начальный ключ не может быть больше конечного");
                MyTreeMap<K, V> sub = new MyTreeMap<K, V>(comparator);
                FillSubMap(sub, root, start, end);
                return sub;
            }

            private void FillSubMap(MyTreeMap<K, V> sub, Node<K, V> node, K start, K end)
            {
                if (node == null) return;
                int compareResultStart = comparator.Compare(node.Key, start);
                int compareResultEnd = comparator.Compare(node.Key, end);

                if (compareResultStart > 0)
                    FillSubMap(sub, node.Left, start, end);

                if (compareResultStart >= 0 && compareResultEnd < 0)
                    sub.Put(node.Key, node.Value);

                if (compareResultEnd < 0)
                    FillSubMap(sub, node.Right, start, end);
            }

            public MyTreeMap<K, V> TailMap(K start)
            {
                MyTreeMap<K, V> tail = new MyTreeMap<K, V>(comparator);
                FillTailMap(tail, root, start);
                return tail;
            }

            private void FillTailMap(MyTreeMap<K, V> tail, Node<K, V> node, K start)
            {
                if (node == null) return;
                int compareResult = comparator.Compare(node.Key, start);

                if (compareResult > 0)
                {
                    FillTailMap(tail, node.Left, start);
                    tail.Put(node.Key, node.Value);
                }
                    
                FillTailMap(tail, node.Right, start);
            }

            public KeyValuePair<K, V>? LowerEntry(K key)
            {
                Node<K, V> current = root;
                Node<K, V> candidate = null;

                while (current != null)
                {
                    int cmp = comparator.Compare(key, current.Key);

                    if (cmp > 0)
                    {
                        candidate = current;
                        current = current.Right;
                    }
                    else
                    {
                        current = current.Left;
                    }
                }

                if (candidate == null) return null;
                return new KeyValuePair<K, V>(candidate.Key, candidate.Value);
            }

            public KeyValuePair<K, V>? FloorEntry(K key)
            {
                Node<K, V> current = root;
                Node<K, V> candidate = null;

                while (current != null)
                {
                    int cmp = comparator.Compare(key, current.Key);

                    if (cmp == 0)
                        return new KeyValuePair<K, V>(current.Key, current.Value);
                    
                    if (cmp > 0)
                    {
                        candidate = current;
                        current = current.Right;
                    }
                    else
                    {
                        current = current.Left;
                    }
                }

                if (candidate == null) return null;
                return new KeyValuePair<K, V>(candidate.Key, candidate.Value);
            }

            public KeyValuePair<K, V>? HigherEntry(K key)
            {
                Node<K, V> current = root;
                Node<K, V> candidate = null;

                while (current != null)
                {
                    int cmp = comparator.Compare(key, current.Key);

                    if (cmp < 0)
                    {
                        candidate = current;
                        current = current.Left;
                    }
                    else
                    {
                        current = current.Right;
                    }
                }

                if (candidate == null) return null;
                return new KeyValuePair<K, V>(candidate.Key, candidate.Value);
            }

            public KeyValuePair<K, V>? CeilingEntry(K key)
            {
                Node<K, V> current = root;
                Node<K, V> candidate = null;

                while (current != null)
                {
                    int cmp = comparator.Compare(key, current.Key);

                    if (cmp == 0)
                        return new KeyValuePair<K, V>(current.Key, current.Value);

                    if (cmp < 0)
                    {
                        candidate = current;
                        current = current.Left;
                    }
                    else
                    {
                        current = current.Right;
                    }
                }

                if (candidate == null) return null;
                return new KeyValuePair<K, V>(candidate.Key, candidate.Value);
            }

            public K LowerKey(K key)
            {
                var entry = LowerEntry(key);
                if (!entry.HasValue) throw new InvalidOperationException("Нет такого ключа");
                return entry.Value.Key;
            }

            public K FloorKey(K key)
            {
                var entry = FloorEntry(key);
                if (!entry.HasValue) throw new InvalidOperationException("Нет такого ключа");
                return entry.Value.Key;
            }

            public K HigherKey(K key)
            {
                var entry = HigherEntry(key);
                if (!entry.HasValue) throw new InvalidOperationException("Нет такого ключа");
                return entry.Value.Key;
            }

            public K CeilingKey(K key)
            {
                var entry = CeilingEntry(key);
                if (!entry.HasValue) throw new InvalidOperationException("Нет такого ключа");
                return entry.Value.Key;
            }

            public KeyValuePair<K, V>? PollFirstEntry()
            {
                if (root == null) return null;
                KeyValuePair<K, V> firstNode = FirstEntry().Value;
                Remove(firstNode.Key);
                return firstNode;
            }

            public KeyValuePair<K, V>? PollLastEntry()
            {
                if (root == null) return null;
                KeyValuePair<K, V> lastNode = LastEntry().Value;
                Remove(lastNode.Key);
                return lastNode;
            }

            public KeyValuePair<K, V>? FirstEntry()
            {
                if (root == null) return null;
                Node<K, V> node = root;

                while (node.Left != null)
                {
                    node = node.Left;
                }

                return new KeyValuePair<K, V>(node.Key, node.Value);
            }

            public KeyValuePair<K, V>? LastEntry()
            {
                if (root == null) return null;
                Node<K, V> node = root;

                while (node.Right != null)
                {
                    node = node.Right;
                }

                return new KeyValuePair<K, V>(node.Key, node.Value);
            }
        }

        static void Main(string[] args)
        {
        }
    }
}
