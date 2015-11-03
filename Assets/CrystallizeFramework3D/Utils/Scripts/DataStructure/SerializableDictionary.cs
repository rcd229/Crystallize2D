using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SerializableDictionary<K, V> where V : ISerializableDictionaryItem<K> {

    bool changed = true;
    Dictionary<K, V> dictionary = new Dictionary<K, V>();

	public List<V> Items { get; set; }

	public SerializableDictionary(){
		Items = new List<V> ();
	}

	public V Get(K key){
        if (changed) {
            dictionary = new Dictionary<K, V>();
            foreach (var item in Items) {
                dictionary.Add(item.Key, item);
            }
            changed = false;
            //Debug.Log("Constructed dictionary: " + this);
        }

        if (dictionary.ContainsKey(key)) {
            return dictionary[key];
        }
        return default(V);
        //return (from v in Items where v.Key.Equals(key) select v).FirstOrDefault ();
	}
	
	public void Remove(K key){
		var i = Get (key);
		if (i != null) {
			Items.Remove(i);
            changed = true;
		}
	}
	
	public void Add(V item){
		var i = Get (item.Key);
		if (i != null) {
			Debug.LogError("Item ID: " + item.Key + " already exists.");
			return;
		}
		Items.Add (item);
        changed = true;
	}

    public void AddRange(IEnumerable<V> items) {
        foreach (var i in items) {
            Add(i);
        }
    }
	
	public void Set(V item){
		Remove (item.Key);
		Add (item);
        changed = true;
	}

    public bool ContainsKey(K key) {
        return Get(key) != null;
    }

}
