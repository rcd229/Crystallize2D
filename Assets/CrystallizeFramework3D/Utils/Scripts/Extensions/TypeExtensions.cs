using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public static class TypeExtensions {

    public static IEnumerable<MethodInfo> GetMethodsBySig(this Type type, Type returnType, params Type[] parameterTypes) {
        return type.GetMethods().Where((m) => {
            if (m.ReturnType != returnType) return false;
            var parameters = m.GetParameters();
            if ((parameterTypes == null || parameterTypes.Length == 0))
                return parameters.Length == 0;
            if (parameters.Length != parameterTypes.Length)
                return false;
            for (int i = 0; i < parameterTypes.Length; i++) {
                if (parameters[i].ParameterType != parameterTypes[i])
                    return false;
            }
            return true;
        });
    }

    public static IEnumerable<MemberInfo> GetFieldsAndProperties<T>(this Type t, BindingFlags flag) {
        return t.GetFields(flag)
            .Cast<MemberInfo>()
            .Concat(t.GetProperties(flag))
            .Where(f => typeof(T).IsAssignableFrom(f.GetUnderlyingType()));
    }

    public static IEnumerable<T> GetFieldAndPropertyValues<T>(this Type t, object obj, bool includeEnumerable = true) {
        var flags = obj == null ?  BindingFlags.Static | BindingFlags.Public : BindingFlags.Instance | BindingFlags.Public;
        var values = from m in t.GetFieldsAndProperties<T>(flags)
                     select (T)m.GetMemberValue(obj);
        if (includeEnumerable) {
            var enumVals = from m in t.GetFieldsAndProperties<IEnumerable<T>>(BindingFlags.Instance | BindingFlags.Public)
                           from e in m.GetMemberValue(obj) as IEnumerable<T>
                           select (T)e;
            return values.Concat(enumVals);
        } else {
            return values;
        }
    }

    public static IEnumerable<T> GetStaticFieldAndPropertyValues<T>(this Type t, bool includeEnumerable = true) {
        var values = from m in t.GetFieldsAndProperties<T>(BindingFlags.Static | BindingFlags.Public)
                     select (T)m.GetMemberValue(null);
        if (includeEnumerable) {
            var enumVals = from m in t.GetFieldsAndProperties<IEnumerable<T>>(BindingFlags.Static | BindingFlags.Public)
                           from e in m.GetMemberValue(null) as IEnumerable<T>
                           select (T)e;
            return values.Concat(enumVals);
        } else {
            return values;
        }
    }

    public static Type GetUnderlyingType(this MemberInfo member) {
        switch (member.MemberType) {
            case MemberTypes.Event:
                return ((EventInfo)member).EventHandlerType;
            case MemberTypes.Field:
                return ((FieldInfo)member).FieldType;
            case MemberTypes.Method:
                return ((MethodInfo)member).ReturnType;
            case MemberTypes.Property:
                return ((PropertyInfo)member).PropertyType;
            default:
                throw new ArgumentException
                (
                 "Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo"
                );
        }
    }

    public static object GetMemberValue(this MemberInfo member, object obj) {
        if (member is PropertyInfo) {
            return ((PropertyInfo)member).GetValue(obj, new object[0]);
        } else if (member is FieldInfo) {
            return ((FieldInfo)member).GetValue(obj);
        } else {
            throw new ArgumentException
                (
                 "Input MemberInfo must be if type FieldInfo or PropertyInfo"
                );
        }
    }

    public static string GetName(Action<object> act) {
        return act.Method.Name;
    }

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
        foreach (T element in source) {
            action(element);
        }
    }

}
