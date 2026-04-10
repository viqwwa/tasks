using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task19
{
    class Node<K, V>
    {
        public K Key { get; set; }
        public V Value { get; set; }
        public Node<K, V> Left { get; set; }
        public Node<K, V> Right { get; set; }
        public Node<K, V> Parent { get; set; }
        public bool Color { get; set; }
        public const bool RED = true;
        public const bool BLACK = false;

        public Node(K key, V value)
        {
            Key = key;
            Value = value;
            Color = RED;
        }
    }

    class SizeCounter
    {
        public int Value;
    }

    internal class MyTreeMap<K, V> where K : IComparable<K>
    {
        public IComparer<K> Comparator { get; }
        public Node<K, V> Root { get; set; }
        private SizeCounter size;

        public MyTreeMap()
        {
            Comparator = Comparer<K>.Default;
            Root = null;
            size.Value = 0;
        }

        public MyTreeMap(IComparer<K> comparator)
        {
            Comparator = comparator;
            Root = null;
            size.Value = 0;
        }

        public MyTreeMap(IComparer<K> comparator, Node<K, V> root, int size)
        {
            Comparator = comparator;
            Root = root;
            this.size.Value = size;
        }

        public void Clear()
        {
            Root = null;
            size.Value = 0;
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

        public Node<K, V> SearchKey(K key)
        {
            Node<K, V> node = Root;

            while (node != null)
            {
                int compareResult = Comparator.Compare(key, node.Key);

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
                return SearchValue(genericValue, Root);
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
            FillEntrySet(Root, set);
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
            return size.Value == 0;
        }

        public ISet<K> KeySet()
        {
            var entries = EntrySet();
            return new HashSet<K>(entries.Select(e => e.Key));
        }

        public List<K> KeyList(K low, bool lowAct, bool lowIncl,
                K high, bool highAct, bool highIncl)
        {
            List<K> keys = new List<K>();
            FillListInRange(Root, keys, low, lowAct, lowIncl, high, highAct, highIncl);
            return keys;
        }

        public void FillListInRange(Node<K, V> node, List<K> list,
                K low, bool lowAct, bool lowIncl,
                K high, bool highAct, bool highIncl)
        {
            if (node == null) return;

            int cmpLow = lowAct ? Comparator.Compare(node.Key, low) : 1;
            int cmpHigh = highAct ? Comparator.Compare(node.Key, high) : -1;
            bool inLow = !lowAct || (lowIncl ? cmpLow >= 0 : cmpLow > 0);
            bool inHigh = !highAct || (highIncl ? cmpHigh <= 0 : cmpHigh < 0);

            if (inLow)
                FillListInRange(node.Left, list, low, lowAct, lowIncl, high, highAct, highIncl);

            if (inLow && inHigh)
                list.Add(node.Key);

            if (inHigh)
                FillListInRange(node.Right, list, low, lowAct, lowIncl, high, highAct, highIncl);
        }

        public void Put(K key, V value)
        {
            if (key == null) throw new ArgumentNullException("Ключ не может быть null");

            Node<K, V> insertedNode = null;
            Root = Put(Root, key, value, null, ref insertedNode);

            if (insertedNode != null)
            {
                BalancePut(insertedNode);
            }
        }

        private Node<K, V> Put(Node<K, V> node, K key, V value, Node<K, V> parent, ref Node<K, V> insertedNode)
        {
            if (node == null)
            {
                size.Value++;
                insertedNode = new Node<K, V>(key, value);
                insertedNode.Parent = parent;
                return insertedNode;
            }

            int compareResult = Comparator.Compare(key, node.Key);

            if (compareResult == 0)
            {
                node.Value = value;
            }
            else if (compareResult < 0)
            {
                node.Left = Put(node.Left, key, value, node, ref insertedNode);
            }
            else
            {
                node.Right = Put(node.Right, key, value, node, ref insertedNode);
            }

            return node;
        }

        private void BalancePut(Node<K, V> node)
        {
            while (node != null && node != Root && node.Parent.Color == Node<K, V>.RED)
            {
                Node<K, V> grandParent = node.Parent.Parent;

                if (node.Parent == grandParent.Left)
                {
                    Node<K, V> uncle = grandParent.Right;

                    if (IsRed(uncle))
                    {
                        node.Parent.Color = Node<K, V>.BLACK;
                        uncle.Color = Node<K, V>.BLACK;
                        grandParent.Color = Node<K, V>.RED;
                        node = grandParent;
                    }
                    else
                    {
                        if (node == node.Parent.Right)
                        {
                            node = node.Parent;
                            LeftRotate(node);
                        }

                        node.Parent.Color = Node<K, V>.BLACK;
                        grandParent.Color = Node<K, V>.RED;
                        RightRotate(grandParent);
                    }
                }
                else
                {
                    Node<K, V> uncle = grandParent.Left;

                    if (IsRed(uncle))
                    {
                        node.Parent.Color = Node<K, V>.BLACK;
                        uncle.Color = Node<K, V>.BLACK;
                        grandParent.Color = Node<K, V>.RED;
                        node = grandParent;
                    }
                    else
                    {
                        if (node == node.Parent.Left)
                        {
                            node = node.Parent;
                            RightRotate(node);
                        }

                        node.Parent.Color = Node<K, V>.BLACK;
                        grandParent.Color = Node<K, V>.RED;
                        LeftRotate(grandParent);
                    }
                }
            }

            Root.Color = Node<K, V>.BLACK;
        }

        public void Remove(K key)
        {
            Node<K, V> z = Root;
            while (z != null)
            {
                int cmp = Comparator.Compare(key, z.Key);
                if (cmp == 0) break;
                z = cmp < 0 ? z.Left : z.Right;
            }

            if (z == null) return;

            Node<K, V> x;
            Node<K, V> y = z;
            bool yOriginalColor = y.Color;

            if (z.Left == null)
            {
                x = z.Right;
                Transplant(z, z.Right);
            }
            else if (z.Right == null)
            {
                x = z.Left;
                Transplant(z, z.Left);
            }
            else
            {
                y = z.Right;
                while (y.Left != null) y = y.Left;

                yOriginalColor = y.Color;
                x = y.Right; 

                if (y.Parent == z)
                {
                    if (x != null) x.Parent = y;
                }
                else
                {
                    Transplant(y, y.Right);
                    y.Right = z.Right;
                    y.Right.Parent = y;
                }

                Transplant(z, y);
                y.Left = z.Left;
                y.Left.Parent = y;
                y.Color = z.Color;
            }

            size.Value--;

            if (yOriginalColor == Node<K, V>.BLACK)
            {
                BalanceRemove(x, x == null ? (y.Parent ?? Root) : x.Parent);
            }
        }

        private void Transplant(Node<K, V> u, Node<K, V> v)
        {
            if (u.Parent == null)
            {
                Root = v;
            }
            else if (u == u.Parent.Left)
            {
                u.Parent.Left = v;
            }
            else
            {
                u.Parent.Right = v;
            }

            if (v != null)
            {
                v.Parent = u.Parent;
            }
        }

        private void BalanceRemove(Node<K, V> node, Node<K, V> parent)
        {
            while (node != Root && !IsRed(node))
            {
                if (node == parent.Left)
                {
                    Node<K, V> brother = parent.Right;

                    if (IsRed(brother))
                    {
                        brother.Color = Node<K, V>.BLACK;
                        parent.Color = Node<K, V>.RED;
                        LeftRotate(parent);
                        brother = parent.Right;
                    }

                    if (!IsRed(brother.Left) && !IsRed(brother.Right))
                    {
                        brother.Color = Node<K, V>.RED;
                        node = parent;
                        parent = node.Parent;
                    }
                    else
                    {
                        if (!IsRed(brother.Right))
                        {
                            brother.Left.Color = Node<K, V>.BLACK;
                            brother.Color = Node<K, V>.RED;
                            RightRotate(brother);
                            brother = parent.Right;
                        }

                        brother.Color = parent.Color;
                        parent.Color = Node<K, V>.BLACK;
                        brother.Right.Color = Node<K, V>.BLACK;
                        LeftRotate(parent);
                        node = Root;
                    }
                }
                else
                {
                    Node<K, V> brother = parent.Left;

                    if (IsRed(brother))
                    {
                        brother.Color = Node<K, V>.BLACK;
                        parent.Color = Node<K, V>.RED;
                        RightRotate(parent);
                        brother = parent.Left;
                    }

                    if (!IsRed(brother.Right) && !IsRed(brother.Left))
                    {
                        brother.Color = Node<K, V>.RED;
                        node = parent;
                    }
                    else
                    {
                        if (!IsRed(brother.Left))
                        {
                            brother.Right.Color = Node<K, V>.BLACK;
                            brother.Color = Node<K, V>.RED;
                            LeftRotate(brother);
                            brother = parent.Left;
                        }

                        brother.Color = parent.Color;
                        parent.Color = Node<K, V>.BLACK;
                        brother.Left.Color = Node<K, V>.BLACK;
                        RightRotate(parent);
                        node = Root;
                    }
                }
            }

            if (node != null) node.Color = Node<K, V>.BLACK;
        }

        private bool IsRed(Node<K, V> node)
        {
            if (node == null) return false;
            return node.Color == Node<K, V>.RED;
        }

        private void LeftRotate(Node<K, V> node)
        {
            Node<K, V> newRoot = node.Right;
            node.Right = newRoot.Left;

            if (newRoot.Left != null)
            {
                newRoot.Left.Parent = node;
            }

            newRoot.Parent = node.Parent;

            if (node.Parent == null)
            {
                Root = newRoot;
            }
            else if (node == node.Parent.Left)
            {
                node.Parent.Left = newRoot;
            }
            else
            {
                node.Parent.Right = newRoot;
            }

            newRoot.Left = node;
            node.Parent = newRoot;
        }

        private void RightRotate(Node<K, V> node)
        {
            Node<K, V> newRoot = node.Left;
            node.Left = newRoot.Right;

            if (newRoot.Right != null)
            {
                newRoot.Right.Parent = node;
            }

            newRoot.Parent = node.Parent;

            if (node.Parent == null)
            {
                Root = newRoot;
            }
            else if (node == node.Parent.Right)
            {
                node.Parent.Left = newRoot;
            }
            else
            {
                node.Parent.Right = newRoot;
            }

            newRoot.Right = node;
            node.Parent = newRoot;
        }

        public int Size => size.Value;

        public int CountInRange(Node<K, V> node,
                        K low, bool lowAct, bool lowIncl,
                        K high, bool highAct, bool highIncl)
        {
            if (node == null) return 0;

            int count = 0;
            int cmpLow = lowAct ? Comparator.Compare(node.Key, low) : 1;
            int cmpHigh = highAct ? Comparator.Compare(node.Key, high) : -1;
            bool inLow = !lowAct || (lowIncl ? cmpLow >= 0 : cmpLow > 0);
            bool inHigh = !highAct || (highIncl ? cmpHigh <= 0 : cmpHigh < 0);

            if (inLow && inHigh)
            {
                count = 1;
            }

            if (inLow)
            {
                count += CountInRange(node.Left, low, lowAct, lowIncl, high, highAct, highIncl);
            }

            if (inHigh)
            {
                count += CountInRange(node.Right, low, lowAct, lowIncl, high, highAct, highIncl);
            }

            return count;
        }


        public K FirstKey(K low, bool lowAct, bool lowIncl, K high, bool highAct, bool highIncl)
        {
            var firstEntry = FirstEntry(low, lowAct, lowIncl, high, highAct, highIncl);
            if (firstEntry == null) throw new InvalidOperationException("Дерево пустое");
            return firstEntry.Value.Key;
        }

        public K LastKey(K low, bool lowAct, bool lowIncl, K high, bool highAct, bool highIncl)
        {
            var firstEntry = LastEntry(low, lowAct, lowIncl, high, highAct, highIncl);
            if (firstEntry == null) throw new InvalidOperationException("Дерево пустое");
            return firstEntry.Value.Key;
        }

        public MyTreeMap<K, V> HeadMap(K end)
        {
            MyTreeMap<K, V> head = new MyTreeMap<K, V>(Comparator);
            FillHeadMap(head, Root, end);
            return head;
        }

        private void FillHeadMap(MyTreeMap<K, V> head, Node<K, V> node, K end)
        {
            if (node == null) return;
            int compareResult = Comparator.Compare(node.Key, end);

            FillHeadMap(head, node.Left, end);

            if (compareResult < 0)
            {
                head.Put(node.Key, node.Value);
                FillHeadMap(head, node.Right, end);
            }
        }

        public MyTreeMap<K, V> SubMap(K start, K end)
        {
            MyTreeMap<K, V> sub = new MyTreeMap<K, V>(Comparator);
            FillSubMap(sub, Root, start, end);
            return sub;
        }

        private void FillSubMap(MyTreeMap<K, V> sub, Node<K, V> node, K start, K end)
        {
            if (node == null) return;
            int compareResultStart = Comparator.Compare(node.Key, start);
            int compareResultEnd = Comparator.Compare(node.Key, end);

            if (compareResultStart > 0)
                FillSubMap(sub, node.Left, start, end);

            if (compareResultStart >= 0 && compareResultEnd < 0)
                sub.Put(node.Key, node.Value);

            if (compareResultEnd < 0)
                FillSubMap(sub, node.Right, start, end);
        }

        public MyTreeMap<K, V> TailMap(K start)
        {
            MyTreeMap<K, V> tail = new MyTreeMap<K, V>(Comparator);
            FillTailMap(tail, Root, start);
            return tail;
        }

        private void FillTailMap(MyTreeMap<K, V> tail, Node<K, V> node, K start)
        {
            if (node == null) return;
            int compareResult = Comparator.Compare(node.Key, start);

            if (compareResult > 0)
            {
                FillTailMap(tail, node.Left, start);
                tail.Put(node.Key, node.Value);
            }

            FillTailMap(tail, node.Right, start);
        }

        public KeyValuePair<K, V>? LowerEntry(K key)
        {
            Node<K, V> current = Root;
            Node<K, V> candidate = null;

            while (current != null)
            {
                int cmp = Comparator.Compare(key, current.Key);

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
            Node<K, V> current = Root;
            Node<K, V> candidate = null;

            while (current != null)
            {
                int cmp = Comparator.Compare(key, current.Key);

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
            Node<K, V> current = Root;
            Node<K, V> candidate = null;

            while (current != null)
            {
                int cmp = Comparator.Compare(key, current.Key);

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
            Node<K, V> current = Root;
            Node<K, V> candidate = null;

            while (current != null)
            {
                int cmp = Comparator.Compare(key, current.Key);

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

        public object LowerKey(K key)
        {
            var entry = LowerEntry(key);
            if (!entry.HasValue) return null;
            return entry.Value.Key;
        }

        public object FloorKey(K key)
        {
            var entry = FloorEntry(key);
            if (!entry.HasValue) return null;
            return entry.Value.Key;
        }

        public object HigherKey(K key)
        {
            var entry = HigherEntry(key);
            if (!entry.HasValue) return null;
            return entry.Value.Key;
        }

        public object CeilingKey(K key)
        {
            var entry = CeilingEntry(key);
            if (!entry.HasValue) return null;
            return entry.Value.Key;
        }

        public KeyValuePair<K, V>? PollFirstEntry(K low, bool lowAct, bool lowIncl, K high, bool highAct, bool highIncl)
        {
            if (Root == null) return null;
            KeyValuePair<K, V> firstNode = FirstEntry(low, lowAct, lowIncl, high, highAct, highIncl).Value;
            Remove(firstNode.Key);
            return firstNode;
        }

        public KeyValuePair<K, V>? PollLastEntry(K low, bool lowAct, bool lowIncl, K high, bool highAct, bool highIncl)
        {
            if (Root == null) return null;
            KeyValuePair<K, V> lastNode = LastEntry(low, lowAct, lowIncl, high, highAct, highIncl).Value;
            Remove(lastNode.Key);
            return lastNode;
        }

        public KeyValuePair<K, V>? FirstEntry(K low, bool lowAct, bool lowIncl, K high, bool highAct, bool highIncl)
        {
            if (Root == null) return null;
            Node<K, V> node = Root;

            int cmpLow = lowAct ? Comparator.Compare(node.Key, low) : 1;
            bool inLow = !lowAct || (lowIncl ? cmpLow >= 0 : cmpLow > 0);

            while (node.Left != null && inLow)
            {
                node = node.Left;
                cmpLow = lowAct ? Comparator.Compare(node.Key, low) : 1;
                inLow = !lowAct || (lowIncl ? cmpLow >= 0 : cmpLow > 0);
            }

            int cmpHigh = highAct ? Comparator.Compare(node.Key, high) : -1;
            bool inHigh = !highAct || (highIncl ? cmpHigh <= 0 : cmpHigh < 0);

            if (inHigh) 
                return new KeyValuePair<K, V>(node.Key, node.Value);

            return null;
        }

        public KeyValuePair<K, V>? LastEntry(K low, bool lowAct, bool lowIncl, K high, bool highAct, bool highIncl)
        {
            if (Root == null) return null;
            Node<K, V> node = Root;

            int cmpHigh = highAct ? Comparator.Compare(node.Key, high) : -1;
            bool inHigh = !highAct || (highIncl ? cmpHigh <= 0 : cmpHigh < 0);

            while (node.Right != null && inHigh)
            {
                node = node.Right;
                cmpHigh = highAct ? Comparator.Compare(node.Key, high) : 1;
                inHigh = !highAct || (highIncl ? cmpHigh >= 0 : cmpHigh > 0);
            }

            int cmpLow = lowAct ? Comparator.Compare(node.Key, low) : 1;
            bool inLow = !lowAct || (lowIncl ? cmpLow >= 0 : cmpLow > 0);

            if (inLow)
                return new KeyValuePair<K, V>(node.Key, node.Value);

            return null;
        }
    }
}
