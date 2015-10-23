using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ResetDialogueAnimation : DialogueAnimation {

    public static List<Action> resetActions = new List<Action>();

    public override void Play(GameObject actor) {
        BlackScreenUI.Instance.FadeOut(0.5f, AfterFadeOut);
    }

    public override DialogueAnimation GetInstance() {
        return new ResetDialogueAnimation();
    }

    void AfterFadeOut(object sender, object args) {
        BlackScreenUI.Instance.FadeIn(0.5f, null);
        //Debug.Log("Resetting");
        foreach (var ra in resetActions) {
            ra.Raise();
        }
        resetActions = new List<Action>();
        Exit();
    }

}
