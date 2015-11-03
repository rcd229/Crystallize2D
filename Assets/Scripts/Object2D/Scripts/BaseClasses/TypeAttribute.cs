using UnityEngine;
using System;
using System.Collections;

public class TypeAttribute : Attribute {
    public Type Type { get; private set; }

    public TypeAttribute(Type type) {
        Type = type;
    }
}
