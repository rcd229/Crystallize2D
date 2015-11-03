using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ThingResourceManager2D {
    static ThingResourceManager2D _instance;
    public static ThingResourceManager2D Instance {
        get {
            if (_instance == null) {
                Initialize();
            }
            return _instance;
        }
    }

    public static void Initialize() {
        _instance = new ThingResourceManager2D();
        _instance.Start();
    }

    Dictionary<Guid, SceneThing2D> resources = new Dictionary<Guid, SceneThing2D>();

    void Start() {
        foreach (var thing in ThingLoader2D.Instance.LoadAll()) {
            Add(thing);
        }
    }

    public void Add(ThingInstance2D thing) {
        if (resources.ContainsKey(thing.Guid)) {
            GameObject.Destroy(resources[thing.Guid].gameObject);
        }

        var instance = GameObjectUtil.GetResourceInstanceFromAttribute<SceneThing2D>();
        instance.Initialize(thing);
        ThingLoader2D.Instance.Save(instance);
        resources[thing.Guid] = instance;
    }

    public void Remove(ThingInstance2D thing) {
        if (resources.ContainsKey(thing.Guid)) {
            GameObject.Destroy(resources[thing.Guid].gameObject);
            resources.Remove(thing.Guid);
            ThingLoader2D.Instance.Delete(thing);
        }
    }
}