using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Reflection;

public abstract class FrameworkInitializer {

    static FrameworkInitializer() {
        var initializers = from a in AppDomain.CurrentDomain.GetAssemblies()
                           from t in Assembly.GetCallingAssembly().GetTypes()
                           where typeof(FrameworkInitializer).IsAssignableFrom(t)
                           select t;
        foreach (var t in initializers) {
            var instance = (FrameworkInitializer)Activator.CreateInstance(t);
            instance.Initialize();
        }
    }

    public static void Get() { }

    protected abstract void Initialize();

}
