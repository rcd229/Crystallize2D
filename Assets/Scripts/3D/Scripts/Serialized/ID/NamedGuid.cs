using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class NamedGuid {

    public static IEnumerable<NamedGuid> GetIDs<T>() {
        var fields = typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public);
        var namedGuids = new List<NamedGuid>();
        foreach (var f in fields) {
            if (f.FieldType == typeof(Guid)) {
                namedGuids.Add(new NamedGuid(f.Name, (Guid)f.GetValue(null)));
            } else if (typeof(UniqueID).IsAssignableFrom(f.FieldType)) {
                namedGuids.Add(new NamedGuid(f.Name, (f.GetValue(null) as UniqueID).guid));
            }
        }

        return namedGuids;
    }

    //public static IEnumerable<NamedGuid> GetIDs(IEnumerable<UniqueID> ids) {
    //    return ids.Select(id => new NamedGuid(id))
    //}

    public static IEnumerable<Guid> GetValues<T>() {
        return GetIDs<T>().Select(n => n.Guid);
    }

    public static Guid GetValue<T>(string idName) {
        return GetIDs<T>().Where(n => n.Name == idName).Select(n => n.Guid).FirstOrDefault();
    }

    public static bool ContainsID<T>(string idName) {
        return GetIDs<T>().Where(n => n.Name == idName).FirstOrDefault() != null;
    }

    public static bool ContainsID<T>(Guid id) {
        return GetIDs<T>().Where(n => n.Guid == id).FirstOrDefault() != null;
    }

    public string Name { get; private set; }
    public Guid Guid { get; private set; }

    public NamedGuid(string name, Guid guid) {
        this.Name = name;
        this.Guid = guid;
    }
}
