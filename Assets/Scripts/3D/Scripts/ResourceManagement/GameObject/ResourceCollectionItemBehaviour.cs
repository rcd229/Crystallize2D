using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class ResourceCollectionItemBehaviour<T> : MonoBehaviour, IResourceCollectionItem
    where T : ResourceCollectionItemBehaviour<T> {
    public void OnEnable() {
        ResourceCollection.GetOrCreate<T>().Add((T)this);
    }

    public void OnDisable() {
        ResourceCollection.GetOrCreate<T>().Remove((T)this);
    }
}
