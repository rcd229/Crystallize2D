using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class EditorResourcePostCompiler : EditorPostCompiler {

    public override void AfterCompile() {
        // TODO: find a better way to initialize static classes
        EffectLibrary.Initialize();

        foreach (var res in GameDataInitializer.requiredResources) {
            if (!GameObjectUtil.ResourceExists(res)) {
                Debug.LogError(string.Format("No prefab found in Resources at [{0}]", res));
            }
        }

        var assembly = Assembly.GetAssembly(typeof(ProcessInitializer));
        var resourceMethods = assembly.GetTypes()
    .SelectMany(type => type.GetMethods())
    .Where(t => Attribute.IsDefined(t, typeof(ResourcePathMethodAttribute)));
        foreach (var m in resourceMethods) {
            if (!m.IsStatic) {
                Debug.LogError("Resource path methods must be static");
                continue;
            } else if (!typeof(IEnumerable<string>).IsAssignableFrom(m.ReturnType)) {
                Debug.LogError("Resource path method must return IEnumerable<string>");
                continue;
            }

            var strings = (IEnumerable<string>)m.Invoke(null, null);
            foreach (var s in strings) {
                if (!GameObjectUtil.ResourceExists(s)) {
                    Debug.LogError(string.Format("No prefab found in Resources at [{0}] for Type [{1}]", s, m.DeclaringType));
                }
            }
        }

        var resourceTypes = from t in assembly.GetTypes()
                            where Attribute.IsDefined(t, typeof(ResourcePathAttribute))
                            select t;
        foreach (var t in resourceTypes) {
            var path = t.GetAttribute<ResourcePathAttribute>().ResourcePath;
            if (!GameObjectUtil.ResourceExists(path)) {
                Debug.LogError(string.Format("No prefab found in Resources at [{0}] for Type [{1}]", path, t));
            } else if (t.IsSubclassOf(typeof(Component))) {
                var r = Resources.Load<GameObject>(path);
                if (!r.GetComponent(t)) {
                    Debug.LogError(string.Format("The prefab for Resource at [{0}] is missing the component [{1}]", path, t));
                }
            }
        }
    }

}
