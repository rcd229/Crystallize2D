using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class ResourceType {

    static readonly Dictionary<Type, int> counters = new Dictionary<Type, int>();
    static readonly Dictionary<Type, List<ResourceType>> types = new Dictionary<Type, List<ResourceType>>();

    [ResourcePathMethod]
    public static IEnumerable<string> GetResourcePaths() {
        var subTypes = from t in Assembly.GetAssembly(typeof(ResourceType)).GetTypes()
                       where t.IsSubclassOf(typeof(ResourceType)) && !t.IsAbstract
                       select t;
        // need to force the static constructor to be called by getting one of the fields
        foreach (var t in subTypes) {
            var fs = t.GetFields(BindingFlags.Static | BindingFlags.Public);
            if (fs.Length == 0) {
                Debug.Log("no fields: " + t);
                continue;
            }
        }

        return types.SelectMany(t => t.Value).Select(t => t.ResourcePath);
    }

    public static T Get<T>(int index) where T : ResourceType {
        return (T)types[typeof(T)].GetSafely(index);
    }

    public static IEnumerable<ResourceType> GetValues<T>() where T : ResourceType {
        return types[typeof(T)];
    } 

    public int Index {
        get {
            return index;
        }
    }

    protected virtual string ResourceDirectory { get { return ""; } }

    public string ResourcePath { get { return ResourceDirectory + name; } }

    protected readonly string name;
    protected readonly int index;

    protected ResourceType(string name) {
        if (!counters.ContainsKey(GetType())) {
            counters[GetType()] = 0;
            types[GetType()] = new List<ResourceType>();
        }

        //Debug.Log(GetType());
        this.index = counters[GetType()];
        this.name = name;
        types[GetType()].Add(this);
    }

    public T LoadResource<T>() where T : UnityEngine.Object  {
        return Resources.Load<T>(ResourcePath);
    }

}

public abstract class ResourceType<T> : ResourceType where T : UnityEngine.Object {

    public T LoadResource()  {
        return Resources.Load<T>(ResourcePath);
    }

    public T GetInstance() {
        return GameObject.Instantiate<T>(LoadResource());
    }

    protected ResourceType(string name) : base(name) { }

}
