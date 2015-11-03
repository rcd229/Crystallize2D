using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class DataContainer : MonoBehaviour {

    public Type Type { get; private set; }

    object data;

    public void Store<T>(T obj) {
        data = obj;
        Type = typeof(T);
    }

    public T Retrieve<T>() {
        return (T)data;
    }

}
