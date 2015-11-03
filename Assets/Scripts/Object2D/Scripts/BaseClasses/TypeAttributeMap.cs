using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T1">The source type must be castable to T1</typeparam>
/// <typeparam name="T2">The target type must be castable to T2</typeparam>
/// <typeparam name="T3">The attribute type</typeparam>
public class TypeAttributeMap<T1, T2, T3> where T3 : TypeAttribute {

    Dictionary<Type, Type> typeMap = new Dictionary<Type, Type>();

    public TypeAttributeMap() {
        var outTypes = from t in Assembly.GetCallingAssembly().GetTypes()
                       where t.HasAttribute<T3>()
                       select t;

        foreach (var targetType in outTypes) {
            var sourceType = targetType.GetAttribute<T3>().Type;
            if (!typeof(T1).IsAssignableFrom(sourceType)) {
                Debug.LogError(sourceType + " must be assignable to " + typeof(T1));
            }

            if (!typeof(T2).IsAssignableFrom(targetType)) {
                Debug.LogError(targetType + " must be assignable to " + typeof(T2));
            } else {
                typeMap[sourceType] = targetType;
            }
        }
    }

    public bool HasTypeFor(Type t) {
        return typeMap.ContainsKey(t);
    }

    public Type GetTypeFor(Type t) {
        if (HasTypeFor(t)) {
            return typeMap[t];
        } else {
            return null;
        }
    }
}
