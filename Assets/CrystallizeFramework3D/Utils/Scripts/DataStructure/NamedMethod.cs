using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class NamedMethod {

    public static IEnumerable<NamedMethod> Collection(params Func<string, string>[] methods) {
        return methods.Select(m => new NamedMethod(m));
    }

    public string Name { get; private set; }
    public Func<string, string> Method { get; private set; }

    public NamedMethod(Func<string, string> method) : this(method.Method.DeclaringType + "_" + method.Method.Name, method) {    }

    public NamedMethod(string name, Func<string, string> method) {
        this.Name = name;
        this.Method = method;
    }

    //public override bool Equals(object obj) {
    //    if (obj is NamedMethod) {
    //        return ((NamedMethod)obj).Name == Name;
    //    }
    //    return false;
    //}

    //public override int GetHashCode() {
    //    return Name.GetHashCode();
    //}

}
