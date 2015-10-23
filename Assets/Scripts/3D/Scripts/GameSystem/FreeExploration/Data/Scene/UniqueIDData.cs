using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public abstract class UniqueIDData : UniqueID {
    public readonly string Name;

    public UniqueIDData(string name, Guid id)
        : base(id) {
            this.Name = name;
    }
}

public abstract class UniqueIDData<T> : UniqueIDData where T : UniqueIDData<T> {

    static protected Dictionary<Guid, UniqueIDData<T>> values;

    static UniqueIDData() {
        //Debug.Log("UniqueIDData constructor: " + typeof(T));
        var fields = (from t in Assembly.GetAssembly(typeof(T)).GetTypes()
                      where typeof(T).IsAssignableFrom(t)
                      select t)
                      .SelectMany(t => t.GetFields(BindingFlags.Static | BindingFlags.Public))
                      .Where(f => typeof(T).IsAssignableFrom(f.FieldType));
        var s = "";
        foreach (var f in fields) {
            var val = f.GetValue(null);
            s += "\n\t" + f.DeclaringType + ": " + f.Name + "; " + f.GetValue(null);
        }
        Debug.Log("For " + typeof(T) + ", found fields: " + GetValues().Count() + s);
    }

    public static T Get(Guid id) {
        if (values != null && values.ContainsKey(id)) {
            return values[id] as T;
        }
        return null;
    }

    public static IEnumerable<T> GetValues() {
        if (values != null) {
            return values.Values.Cast<T>();
        }
        return new T[0];
    }

    public static IEnumerable<NamedGuid> GetNamedGuids() {
        return GetValues().Select(g => new NamedGuid(g.Name, g.guid));
    }

    public UniqueIDData(string name, Guid id)
        : base(name, id) {
        if (values == null) {
            values = new Dictionary<Guid, UniqueIDData<T>>();
        }

        if (values.ContainsKey(guid)) {
            //Debug.LogError(id + " is not unique!");
        } else {
            values[guid] = this;
        }
    }

}
