using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class DictionaryCollectionGameData<T> : UniqueKeySerializableDictionary<T> 
    where T : IHasID, ISerializableDictionaryItem<int>, ISetableKey<int>, new() {

    public T AddNewItem() {
        var j = new T();
        j.ID = GetNextKey();
        Add(j);
        return j;
    }

}
