using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public static class CollectionExtensions {
    public static void Shuffle<T>(this List<T> items) {
        int n = items.Count;
        while (n > 1) {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = items[k];
            items[k] = items[n];
            items[n] = value;
        }
    }

    public static List<T> Randomize<T>(this IEnumerable<T> collection) {
        var items = new List<T>(collection);
        items.Shuffle();
        return items;
    }

    public static T GetRandom<T>(this List<T> collection) {
        return collection[UnityEngine.Random.Range(0, collection.Count)];
    }

    public static T GetRandom<T>(this T[] collection) {
        return collection[UnityEngine.Random.Range(0, collection.Length)];
    }

    public static T GetRandomFromEnumerable<T>(this IEnumerable<T> collection) {
        var c = collection.Count();
        if(c == 0){
            return default(T);
        }
        return collection.ElementAt(UnityEngine.Random.Range(0, c));
    }

    public static List<T> RandomSubsetWithValue<T>(IEnumerable<T> collection, T needed, int total) {
        var set = collection.Randomize();
        set = set.Take(total).ToList();
        if (set.Contains(needed)) {
            set.Remove(needed);
        } else {
            set.RemoveAt(0);
        }
        set.Add(needed);
        set.Shuffle();
        return set;
    }

    public static int GetNeededIndex<T>(List<T> List, T needed) {
        return List.IndexOf(needed);
    }

    public static List<T> RandomSubsetWithValues<T>(IEnumerable<T> collection, IEnumerable<T> needed, int total) {
        var set = collection.Randomize();
        set = set.Take(total - needed.Count()).ToList();
        set.AddRange(needed);
        set.Shuffle();
        return set;
    }

    public static T GetNewRandom<T>(this IEnumerable<T> collection, IEnumerable<T> existing) {
        var collectionSet = new HashSet<T>(collection);
        collectionSet.ExceptWith(existing);
        if (collectionSet.Count > 0) {
            return collectionSet.ElementAt(UnityEngine.Random.Range(0, collectionSet.Count));
        } else {
            return collection.ElementAt(UnityEngine.Random.Range(0, collection.Count()));
        }
    }
}
