using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task21
{
    internal class Program
    {
        public class Entry<K, V>
        {
            public K Key { get; set; }
            public V Value { get; set; }

            public Entry(K key, V value)
            {
                Key = key;
                Value = value;
            }

            public override bool Equals(object obj)
            {
                if (obj is Entry<K, V> other)
                    return EqualityComparer<K>.Default.Equals(Key, other.Key) &&
                           EqualityComparer<V>.Default.Equals(Value, other.Value);
                return false;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;

                    hash = hash * 23 + (Key == null ? 0 : Key.GetHashCode());
                    hash = hash * 23 + (Value == null ? 0 : Value.GetHashCode());

                    return hash;
                }
            }
        }

        class MyHashMap<K, V>
        {
            LinkedList<Entry<K, V>>[] table;
            int size;
            double loadFactor;

            public MyHashMap(int initialCapacity, double loadFactor)
            {
                if (initialCapacity <= 0)
                    throw new ArgumentException("Начальная емкость должна быть больше нуля.");

                if (loadFactor <= 0 || loadFactor >= 1)
                    throw new ArgumentException("LoadFactor должен быть в диапазоне от 0 до 1.");

                size = 0;
                table = new LinkedList<Entry<K, V>>[initialCapacity];
                this.loadFactor = loadFactor;

                for (int i = 0; i < initialCapacity; i++)
                    table[i] = new LinkedList<Entry<K, V>>();
            }

            public MyHashMap() : this(16, 0.75) { }

            public MyHashMap(int initialCapacity) : this(initialCapacity, 0.75) { }

            public void Clear()
            {
                foreach (var bucket in table)
                {
                    if (bucket != null)
                    {
                        bucket.Clear();
                    }
                }

                size = 0;
            }

            public bool ContainsKey(object key)
            {
                if (key == null || !(key is K genericKey)) return false;

                int index = GetIndex(genericKey);

                foreach (var entry in table[index])
                {
                    if (EqualityComparer<K>.Default.Equals(entry.Key, genericKey))
                    {
                        return true;
                    }
                }

                return false;
            }

            private int GetIndex(K key)
            {
                int h1 = key.GetHashCode();
                int h2 = h1 ^ (int)((uint)h1 >> 16);
                return (h2 & 0x7FFFFFFF) % table.Length;
            }

            public bool ContainsValue(object value)
            {
                if (value == null || !(value is V genericValue)) return false;

                for (int i = 0; i < table.Length; i++)
                {
                    foreach (var element in table[i])
                    {
                        if (EqualityComparer<V>.Default.Equals(genericValue, element.Value))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            public HashSet<Entry<K, V>> EntrySet()
            {
                var set = new HashSet<Entry<K, V>>();

                foreach (var bucket in table)
                {
                    foreach (var entry in bucket)
                        set.Add(entry);
                }

                return set;
            }

            public object Get(object key)
            {
                if (key == null || !(key is K genericKey)) return null;

                int index = GetIndex(genericKey);

                foreach (var entry in table[index])
                {
                    if (EqualityComparer<K>.Default.Equals(entry.Key, genericKey))
                    {
                        return entry.Value;
                    }
                }

                return null;
            }

            public bool isEmpty()
            {
                return size == 0;
            }

            public HashSet<K> KeySet()
            {
                var set = new HashSet<K>();

                foreach (var bucket in table)
                {
                    foreach (var entry in bucket)
                        set.Add(entry.Key);
                }

                return set;
            }

            public void Put(K key, V value)
            {
                if (key == null)
                    throw new ArgumentNullException("Ключ не может быть null.");

                int index = GetIndex(key);

                foreach (var entry in table[index])
                {
                    if (EqualityComparer<K>.Default.Equals(entry.Key, key))
                    {
                        entry.Value = value;
                        return;
                    }
                }

                table[index].AddLast(new Entry<K, V>(key, value));
                size++;

                if ((double)size / table.Length >= loadFactor)
                    Resize();
            }

            private void Resize()
            {
                LinkedList<Entry<K, V>>[] oldTable = table;
                table = new LinkedList<Entry<K, V>>[table.Length * 2];

                for (int i = 0; i < table.Length; i++)
                {
                    table[i] = new LinkedList<Entry<K, V>>();
                }

                foreach (var bucket in oldTable)
                {
                    foreach (var entry in bucket)
                    {
                        int index = GetIndex(entry.Key);
                        table[index].AddLast(entry);
                    }
                }
            }

            public bool Remove(object key)
            {
                if (key == null || !(key is K genericKey)) return false;

                int index = GetIndex(genericKey);
                var bucket = table[index];

                var current = bucket.First;
                while (current != null)
                {
                    if (EqualityComparer<K>.Default.Equals(current.Value.Key, genericKey))
                    {
                        bucket.Remove(current);
                        size--;
                        return true;
                    }
                    current = current.Next;
                }

                return false;
            }

            public int Size() => size;
        }

        static void Main(string[] args)
        {
        }
    }
}
