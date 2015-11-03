using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class GuidSerializableDictionary<K, T> : SerializableDictionary<K, T> 
    where T : ISerializableDictionaryItem<K>, ISetableKey<K>, new() 
{

    public GuidSerializableDictionary() : base() { }

    public T GetOrCreateItem(K id) {
        if (!ContainsKey(id)) {
            var t = new T();
            t.SetKey(id);
            Add(t);
        }
        return Get(id);
    }

}
