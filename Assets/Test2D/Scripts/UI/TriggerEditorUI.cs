using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

[ResourcePath("UI/TriggerEditor")]
public class TriggerEditorUI : InteractionEditorUI<TriggerData2D> {
    public override InteractionHostType Type {
        get { return InteractionHostType.Trigger; }
    }
    
    public override void Save() {
        TriggerLoader2D.Instance.Save(target);
    }
}
