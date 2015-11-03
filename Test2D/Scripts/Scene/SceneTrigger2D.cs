using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

[ResourcePath("2D/OneWayTrigger")]
public class SceneTrigger2D : ResourceCollectionItemBehaviour<SceneTrigger2D>, IHasSceneEditor {
    public TriggerData2D Trigger { get; set; }

    public void Initialize(TriggerData2D trigger) {
        this.Trigger = trigger;
        transform.position = (Vector2)trigger.Position;
        transform.rotation = Quaternion.Euler(0, 0, trigger.Orientation);
        transform.localScale = new Vector3(trigger.Size, 1f);
    }

    public ITemporaryUI GetEditor() {
        var f = new UIFactoryRef<TriggerData2D, object>();
        f.Set<TriggerEditorUI>();
        return f.Get(Trigger);
    }
}
