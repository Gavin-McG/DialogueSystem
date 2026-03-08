using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    [Serializable]
    public struct SerializableKeyValuePair<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;
    }
    
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<SerializableKeyValuePair<TKey, TValue>> entries = new();

        private Dictionary<TKey, TValue> dictionary = new();

        //-----------------------------------
        //      Dictionary methods
        //-----------------------------------
        
        public int Count => dictionary.Count;
        public bool IsReadOnly => false;
        
        public ICollection<TKey> Keys => dictionary.Keys;
        public ICollection<TValue> Values => dictionary.Values;

        public void Add(TKey key, TValue value) => dictionary.Add(key, value);
        public void Add(KeyValuePair<TKey, TValue> item) => dictionary.Add(item.Key, item.Value);

        public bool Remove(TKey key) => dictionary.Remove(key);
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (dictionary.TryGetValue(item.Key, out var value) &&
                EqualityComparer<TValue>.Default.Equals(value, item.Value))
            {
                return dictionary.Remove(item.Key);
            }
            return false;
        }

        public void Clear() => dictionary.Clear();
        
        public bool Contains(KeyValuePair<TKey, TValue> item) => dictionary.Contains(item);
        public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);
        
        public bool TryGetValue(TKey key, out TValue value) => dictionary.TryGetValue(key, out value);
        public TValue this[TKey key]
        {
            get => dictionary[key];
            set => dictionary[key] = value;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dictionary.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => dictionary.GetEnumerator();

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));

            if (array.Length - arrayIndex < dictionary.Count)
                throw new ArgumentException("The destination array has insufficient space.");

            foreach (var kvp in dictionary)
            {
                array[arrayIndex++] = kvp;
            }
        }

        //-----------------------------------
        //      Serialization methods
        //-----------------------------------

        public void OnBeforeSerialize()
        {
            entries.Clear();
            foreach (var kvp in dictionary)
            {
                entries.Add(new SerializableKeyValuePair<TKey, TValue>
                {
                    Key = kvp.Key,
                    Value = kvp.Value
                });
            }
        }

        public void OnAfterDeserialize()
        {
            dictionary.Clear();
            foreach (var entry in entries)
            {
                if (!dictionary.ContainsKey(entry.Key))
                    dictionary.Add(entry.Key, entry.Value);
            }
            entries.Clear();
            entries.TrimExcess();
        }
    }
}
