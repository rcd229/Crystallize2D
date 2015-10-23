using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ActorGenerator : MonoBehaviour {

    void Start() {
        foreach (var s in StringExtensions.GetCountSet("Target", 1, 5, 2)) {
            var t = GameObject.Find(s);
            var actor = DialogueActorUtil.GetNewActor(AppearanceLibrary.GetRandomAppearance());
            actor.transform.position = t.transform.position;
            actor.transform.rotation = t.transform.rotation;
        }
    }

}
