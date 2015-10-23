using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

[ResourcePath("UI/ThingEditor")]
public class ThingEditorUI : InteractionEditorUI<ThingInstance2D> {
    public override InteractionHostType Type {
        get { return InteractionHostType.Thing; }
    }
    
    public override void Save() {
        ThingLoader2D.Instance.Save(target);
    }
}
