using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public static class AttributeExtensions {

    public static bool HasAttribute<T>(this Type t) where T : Attribute {
        return Attribute.IsDefined(t, typeof(T));
    }

    public static T GetAttribute<T>(this Type t) where T : Attribute {
        return Attribute.GetCustomAttribute(t, typeof(T)) as T;
    }

}
