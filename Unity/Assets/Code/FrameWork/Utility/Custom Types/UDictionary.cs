using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class UDictionary<K,V> : IDictionary<K,V>, ISerializationCallbackReceiver
{
    #region Fields

    // Used for serialization
    [SerializeField]
    private List<K> keys;

    [SerializeField]
    private List<V> values;

    // When deserialized
    [NonSerialized]
    private Dictionary<K, V> dictionary;

    #endregion

    #region Constructor

    public UDictionary()
    {
        dictionary = new Dictionary<K, V>();
        keys = dictionary.Keys.ToList();
        values = dictionary.Values.ToList();
    }

    #endregion

    #region Serialization

    // Deserialize
    public void OnAfterDeserialize()
    {
        dictionary = new Dictionary<K, V>();

        for(int i =0; i < values.Count; i++)
            dictionary[keys[i]] = values[i];

        //Debug.Log("Deserialized: " + keys.Count + " | " + values.Count);
    }

    // Serialize
    public void OnBeforeSerialize()
    {
        //Debug.Log("Serialization");
        if (dictionary == null)
            dictionary = new Dictionary<K, V>();

        values = dictionary.Values.ToList();
        keys = dictionary.Keys.ToList();
    }

    #endregion

    public void ShallowCopyIn(UDictionary<K,V> copyMe)
    {
        dictionary = new Dictionary<K,V>();

        values = copyMe.dictionary.Values.ToList();
        keys = copyMe.dictionary.Keys.ToList();

        for (int i = 0; i < keys.Count; i++)
            dictionary[keys[i]] = values[i];
    }

    #region Dictionary Implmentation

    public void Add(K key, V value)
    {
        dictionary.Add(key, value);
    }

    public bool ContainsKey(K key)
    {
        return dictionary.ContainsKey(key);
    }

    public ICollection<K> Keys
    {
        get { return dictionary.Keys; }
    }

    public bool Remove(K key)
    {
        return dictionary.Remove(key);
    }

    public bool TryGetValue(K key, out V value)
    {
        return dictionary.TryGetValue(key, out value);
    }

    public ICollection<V> Values
    {
        get { return dictionary.Values; }
    }

    public V this[K key]
    {
        get
        {
            return dictionary[key];
        }
        set
        {
            dictionary[key] = value;
        }
    }

    public IEnumerator GetEnumerator()
    {
        return dictionary.GetEnumerator();
    }

    public void Add(KeyValuePair<K, V> item)
    {
        dictionary.Add(item.Key,item.Value);
    }

    public void Clear()
    {
        dictionary.Clear();
        if (keys.Count == 0)
            return;
        keys.Clear();
        values.Clear();
    }

    public bool Contains(KeyValuePair<K, V> item)
    {
        return dictionary.Contains(item);
    }

    public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
    {
        for(int i = 0; i < array.Length; i++)
        {
            dictionary[array[i].Key] = array[i].Value;
        }
    }

    public int Count
    {
        get { return dictionary.Count; }
    }

    public bool IsReadOnly
    {
        get { return false; }
    }

    public bool Remove(KeyValuePair<K, V> item)
    {
        return dictionary.Remove(item.Key);
    }

    IEnumerator<KeyValuePair<K, V>> IEnumerable<KeyValuePair<K, V>>.GetEnumerator()
    {
        return dictionary.GetEnumerator();
    }

    #endregion
}
