using UnityEngine;
using UnityEditor;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public abstract class EditorPostCompiler {
    
    [InitializeOnLoadMethod]
    static void RunPostCompiler() {
        //Debug.Log("Running post compiler");
        var postCompilers = from p in Assembly.GetAssembly(typeof(EditorPostCompiler)).GetTypes()
                            where p.IsSubclassOf(typeof(EditorPostCompiler))
                            select p;
        foreach (var p in postCompilers) {
            GetInstance(p).AfterCompile();
        }
    }

    public static EditorPostCompiler GetInstance(Type t) {
        if (!t.IsSubclassOf(typeof(EditorPostCompiler))) {
            Debug.LogError("PostCompilers must be subclasses of EditorPostCompiler");
            return null;
        }

        if (t.GetConstructor(Type.EmptyTypes) == null) {
            Debug.LogError("PostCompilers have parameterless constructor");
            return null;
        }

        return Activator.CreateInstance(t) as EditorPostCompiler;
    }

    public static EditorPostCompiler GetInstance<T>() where T : EditorPostCompiler, new() {
        return new T();
    }

    public abstract void AfterCompile();

}
