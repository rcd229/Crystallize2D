using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class NPCAnimationData {
    public static readonly NPCAnimationData Normal = new NPCAnimationData("Stand");
    public static readonly NPCAnimationData Relaxed = new NPCAnimationData("Stand_Relaxed");
    public static readonly NPCAnimationData Crossed = new NPCAnimationData("Stand_Crossed");

    public string StandAnimation { get; private set; }

    public NPCAnimationData() {
        StandAnimation = "Stand";
    }

    NPCAnimationData(string anim) {
        this.StandAnimation = anim;
    }

    public void SetTo(GameObject target) {
        if (!target.GetComponent<SetAnimation>()) {
            target.AddComponent<SetAnimation>();
        }
        target.GetComponent<SetAnimation>().Set(StandAnimation);
    }
}
