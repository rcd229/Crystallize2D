using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class TriggerResourceManager2D {
    static TriggerResourceManager2D _instance;
    public static TriggerResourceManager2D Instance {
        get {
            if (_instance == null) {
                Initialize();
            }
            return _instance;
        }
    }

    public static void Initialize() {
        _instance = new TriggerResourceManager2D();
        _instance.Start();
    }

    Dictionary<Guid, SceneTrigger2D> resources = new Dictionary<Guid, SceneTrigger2D>();

    void Start() {
        foreach (var thing in TriggerLoader2D.Instance.LoadAll()) {
            Add(thing);
        }
    }

    public void Add(TriggerData2D trigger) {
        if (resources.ContainsKey(trigger.Guid)) {
            GameObject.Destroy(resources[trigger.Guid].gameObject);
        }

        var instance = GameObjectUtil.GetResourceInstanceFromAttribute<SceneTrigger2D>();
        instance.Initialize(trigger);
        TriggerLoader2D.Instance.Save(instance);
        resources[trigger.Guid] = instance;
    }
}
