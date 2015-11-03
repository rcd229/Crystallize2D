using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

[ResourcePath("2D/Thing")]
public class SceneThing2D : ResourceCollectionItemBehaviour<SceneThing2D>, IHasSceneEditor {
    public ThingInstance2D Thing { get; set; }

    public void Initialize(ThingInstance2D thing) {
        this.Thing = thing;
        transform.position = (Vector2)thing.Position;
    }

    public ITemporaryUI GetEditor() {
        var ui = GameObjectUtil.GetResourceInstanceFromAttribute<ThingEditorUI>();
        ui.Initialize(Thing, EditEnded, null);
        return ui;
    }

    void EditEnded(object sender, EventArgs args) {
        FloatingNameManager2D.Instance.Add(transform, Thing.Name);
    }
}
