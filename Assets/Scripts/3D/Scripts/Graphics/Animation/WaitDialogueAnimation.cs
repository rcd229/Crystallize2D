using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class WaitDialogueAnimation : DialogueAnimation {

    float waitDuration = 0;

    public WaitDialogueAnimation(float duration) {
        waitDuration = duration;
    }

    public override void Play(GameObject actor) {
        CoroutineManager.Instance.StartCoroutine(PlaySequence());
    }

    public override DialogueAnimation GetInstance() {
        return new WaitDialogueAnimation(waitDuration);
    }

    IEnumerator PlaySequence() {
        yield return new WaitForSeconds(waitDuration);

        Exit();
    }
}
