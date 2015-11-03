using UnityEngine;
using System.Collections;

public interface ISerializableDictionaryItem<T> {

    T Key { get; }

}

public interface ISetableKey<T> {
    void SetKey(T key);
}
