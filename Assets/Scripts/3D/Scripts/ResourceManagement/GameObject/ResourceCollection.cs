using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ResourceCollection {
    static Dictionary<Type, object> resourceCollections = new Dictionary<Type, object>();

    public static ResourceCollection<T> Get<T>() where T : IResourceCollectionItem {
        if (resourceCollections.ContainsKey(typeof(T))) {
            return (ResourceCollection<T>)resourceCollections[typeof(T)];
        } else {
            return null;
        }
    }

    public static ResourceCollection<T> GetOrCreate<T>() where T : IResourceCollectionItem {
        if (!resourceCollections.ContainsKey(typeof(T))) {
            resourceCollections[typeof(T)] = new ResourceCollection<T>();
        } 
        return (ResourceCollection<T>)resourceCollections[typeof(T)];
    }

    protected static void Add<T>(ResourceCollection<T> collection) where T : IResourceCollectionItem {
        resourceCollections[typeof(T)] = collection;
    }
}

public class ResourceCollection<T> : IEnumerable<T> where T : IResourceCollectionItem {
    HashSet<T> items = new HashSet<T>();

    public event EventHandler<EventArgs<T>> OnItemAdded;
    public event EventHandler<EventArgs<T>> OnItemRemoved;

    public void Add(T item) {
        items.Add(item);
        OnItemAdded.Raise(this, new EventArgs<T>(item));
    }

    public void Remove(T item) {
        items.Remove(item);
        OnItemRemoved.Raise(this, new EventArgs<T>(item));
    }

    public IEnumerator<T> GetEnumerator() {
        return items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return items.GetEnumerator();
    }
}
