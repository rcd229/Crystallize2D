using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DebugMenuListener : MonoBehaviour {

    static DebugMenuListener _instance;

    static HashSet<NamedMethod> contextMethods = new HashSet<NamedMethod>();

    public static IEnumerable<NamedMethod> GetMethods() {
        return contextMethods;
    }

    public static void AddContextMethods(IEnumerable<NamedMethod> methods) {
        contextMethods.UnionWith(methods);
    }

    public static void RemoveContextMethods(IEnumerable<NamedMethod> methods) {
        contextMethods.ExceptWith(methods);
    }

    ITemporaryUI debugMenu;

    void Awake() {
        if (_instance != null) {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update() {
        if (GameSettings.Instance.IsDebug) {
            if (Input.GetKeyDown(KeyCode.Slash) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) {
                debugMenu.CloseIfNotNull();
                debugMenu = UILibrary.DebugMenu.Get(null);
            }
        }
    }

}
