using UnityEngine;
using System.Collections;

public class DialogueAnimationInitArgs {
    public DialogueSequence Dialogue { get; set; }
    public DialogueState State { get; set; }

    public DialogueAnimationInitArgs(DialogueSequence dialogue, DialogueState state) {
        Dialogue = dialogue;
        State = state;
    }
}

public class AnimationDialogueElementProcess : IProcess<DialogueState, DialogueState> {

    DialogueState state;

    public ProcessExitCallback OnExit { get; set; }

    public void ForceExit() {
        Exit();
    }

    public void Initialize(DialogueState param1) {
        state = param1;
        var anim = ((AnimationDialogueElement)state.GetElement()).Animation.GetInstance();
        //Debug.Log("Beginning dialogue animation; " + anim + "; " + Time.time);
        anim.OnComplete += anim_OnComplete;
        state.GetTarget().name = "Actor" + state.GetElement().ActorIndex;
        anim.Play(state.GetTarget());
    }

    void anim_OnComplete(object sender, System.EventArgs e) {
        Exit();
    }

    void Exit() {
		var anim = ((AnimationDialogueElement)state.GetElement()).Animation;
		anim.OnComplete -= anim_OnComplete;
        OnExit.Raise(this, new DialogueState(state.GetElement().DefaultNextID, state.Dialogue, state.Context, state.ActorMap, state.GameObjects));
    }

}