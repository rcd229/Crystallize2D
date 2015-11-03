using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class Object2D {
    static Type[] types;
    static Object2D() {
        types = (from t in Assembly.GetAssembly(typeof(Object2D)).GetTypes()
                 where typeof(Object2D).IsAssignableFrom(t)
                 select t)
                .ToArray();
    }

    public static Type[] GetTypes() {
        return types;
    }

    public Guid Guid { get; set; }
    public string Name { get; set; }
    public string PrefabName { get; set; }
    public SerializableVector2 Position { get; set; }

    public Object2D() {
        Guid = Guid.NewGuid();
        this.Name = "New " + GetType().ToString();
    }

    public Object2D(string prefabName, Vector2 position) : this() {
        this.PrefabName = prefabName;
        this.Position = position;
    }

    public void CopyTo(Object2D other) {
        other.Guid = Guid;
        other.Name = Name;
        other.PrefabName = PrefabName;
        other.Position = Position;
    }
}
