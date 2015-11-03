using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TransformPath {
    static Dictionary<string, Transform> rootTransforms = new Dictionary<string, Transform>();

    public static void Add(Transform target, params string[] path) {
        target.SetParent(Get(path), false);
    }

    public static Transform Get(params string[] path) {
        if (path.Length == 0) { return null; }
        return GetTransformRecursively(GetRootTransform(path[0]), path, 1);
    }

    static Transform GetTransformRecursively(Transform root, string[] path, int index) {
        if (index >= path.Length) { return root; }
        return GetTransformRecursively(GetOrCreateChild(root, path[index]), path, index + 1);
    }

    static Transform GetOrCreateChild(Transform root, string name) {
        var child = root.Find(name);
        if (!child) {
            child = new GameObject(name).transform;
            child.SetParent(root, false);
        }
        return child;
    }

    static Transform GetRootTransform(string name) {
        if (!rootTransforms.ContainsKey(name) || !rootTransforms[name]) 
            rootTransforms[name] = new GameObject(name).transform;
        return rootTransforms[name];
    }
}
