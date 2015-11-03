using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class PlayModeController2D : ModeController2D {

    protected override void Update() {
        base.Update();
    }

    protected override void OnLeftClick(RaycastHit2D hit) {
        if (hit.transform.gameObject.GetInterface<IInteractableSceneObject>() != null) {
            hit.transform.gameObject.GetInterface<IInteractableSceneObject>().BeginInteraction(null, null);
        }
    }

    protected override void OnRightClick(RaycastHit2D hit) {
        Debug.Log(hit.transform);
        if (hit.transform.GetComponent<SceneThing2D>()) {
            var t = hit.transform.GetComponent<SceneThing2D>();
            if (!t.Thing.Description.IsEmpty) {
                var phrases = new List<PhraseSequence>();
                phrases.Add(t.Thing.Description);
                GameObjectUtil.GetResourceInstanceFromAttribute<NarrationTextUI>().Initialize(phrases, null, null);
            }
        }
    }

}
